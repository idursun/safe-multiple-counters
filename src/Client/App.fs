module App

open Elmish
open Elmish.React
open Counter

open Fable.Helpers.React.Props
module R = Fable.Helpers.React

open Fable.PowerPack.Fetch
open Fable.Core.JsInterop

importAll "bulma/css/bulma.css"

type Model = {
    counters: Counter.Model list
  }

type Msg =
  | Init of Result<int, exn>
  | CounterMsg of int * Counter.Msg

let init () = 
  let model = { counters = [Counter.init (); Counter.init ()] }
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
                    | CounterMsg (index, m) -> 
                      let _updatedCounters =  model.counters |> List.mapi(fun i c-> if i = index then fst (Counter.update m c) else c)
                      { model with counters = _updatedCounters}, Cmd.none
  model', cmd

let header _ _ = 
  R.nav [ClassName "navbar is-info"; Role "navigation"] [
    R.div [ClassName "navbar-brand"] [
      R.a [ClassName "navbar-item"] [
        R.img [Src "https://bulma.io/images/bulma-logo.png"] 
      ] 
    ]
    R.div [ClassName "navbar-menu"] [
      R.a [ClassName "navbar-item"] [R.str "Home"]
      R.a [ClassName "navbar-item"] [R.str "Configuration"]
      R.a [ClassName "navbar-item"] [R.str "About"]
    ]
  ]

let content model dispatch = 
    R.section [ClassName "section"] [
      R.div [ClassName "container"] [
       R.div [] (model.counters |> List.mapi (fun index m -> view m (fun m -> dispatch (CounterMsg (index, m))))) ]
    ]

let footer _ _ = 
  R.footer [ClassName "footer"] [
    R.div [ClassName "container"] [
      R.div [ClassName "content text-centered"] [
        R.p [] [R.str "footer goes here"]
      ]
    ]
  ]

let view model dispatch =
  R.div [] [
      header model dispatch
      content model dispatch
      footer model dispatch
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
