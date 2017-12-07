module App

open Elmish
open Elmish.React

open Fable.Helpers.React.Props
module R = Fable.Helpers.React

open Fable.PowerPack.Fetch
open Fable.Core.JsInterop

importAll "bulma/css/bulma.css"

open Fulma.Components
open Fulma.Layouts

open Counter
open Components.Header
open Components.Footer

type Model = {
    activePage : string
    counters: Counter.Model list
  }

type Msg =
  | Init of Result<int, exn>
  | HeaderMsg of Components.Header.Msg
  | CounterMsg of int * Counter.Msg

let init () = 
  let model = { activePage = "home"; counters = [Counter.init (); Counter.init ()] }
  let cmd = Cmd.ofPromise 
              (fetchAs<int> "/api/init") 
              [] 
              (Ok >> Init) 
              (Error >> Init)
  model, cmd


let update msg (model : Model) =
  let model', cmd = match msg with
                    | Init (Ok v) -> {model with counters = [v; v]}, Cmd.none
                    | Init (Error _) -> {model with counters = [0; 0]}, Cmd.none
                    | HeaderMsg (GoPage m)  -> {model with activePage = m}, Cmd.none
                    | CounterMsg (index, m) -> 
                      let _updatedCounters =  model.counters |> List.mapi(fun i c-> if i = index then fst (Counter.update m c) else c)
                      { model with counters = _updatedCounters}, Cmd.none
  model', cmd



let content model dispatch = 
    Section.section [] [
       match model.activePage with
       | "home" -> yield R.div [] (model.counters |> List.mapi (fun index m -> view m (fun m -> dispatch (CounterMsg (index, m)))))
       | "test" -> yield R.str "test goes here"
       | "about" -> yield R.str "about goes here"
       | "configuration" -> yield R.str "configuration goes here"
       | _ -> yield R.str "should not happen"
    ]

let view model dispatch =
  R.div [] [
      header model (HeaderMsg >> dispatch)
      content model dispatch
      footerView model dispatch
  ]

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram init update view
#if DEBUG
|> Program.withConsoleTrace
|> Program.withHMR
#endif
|> Program.withReact "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
