module App

open Elmish
open Elmish.React
open Counter

open Fable.Helpers.React.Props
module R = Fable.Helpers.React

open Fable.PowerPack.Fetch
open Fable.Core.JsInterop

importAll "bulma/css/bulma.css"

open Fulma.Components
open Fulma.Layouts

type Model = {
    activePage : string
    counters: Counter.Model list
  }

type Msg =
  | Init of Result<int, exn>
  | GoPage of string
  | CounterMsg of int * Counter.Msg

let init () = 
  let model = { activePage = "Home"; counters = [Counter.init (); Counter.init ()] }
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
                    | GoPage page  -> {model with activePage = page}, Cmd.none
                    | CounterMsg (index, m) -> 
                      let _updatedCounters =  model.counters |> List.mapi(fun i c-> if i = index then fst (Counter.update m c) else c)
                      { model with counters = _updatedCounters}, Cmd.none
  model', cmd

let header _ dispatch = 
  Navbar.navbar [] [
    Navbar.brand_div [] [
      Navbar.item_a [Navbar.Item.props [Href "#"]] []
    ]
    Navbar.item_div [] [
      Navbar.item_a [Navbar.Item.props [OnClick (fun _ -> dispatch (GoPage "home"))]] [R.str "Home"]
      Navbar.item_a [Navbar.Item.props [OnClick (fun _ -> dispatch (GoPage "test"))]] [R.str "Test"]
      Navbar.item_a [Navbar.Item.props [OnClick (fun _ -> dispatch (GoPage "about"))]] [R.str "About"]
      Navbar.item_a [Navbar.Item.props [OnClick (fun _ -> dispatch (GoPage "configuration"))]] [R.str "Configuration"]
    ]
  ]

let content model dispatch = 
    Section.section [] [
       match model.activePage with
       | "home" -> yield R.div [] (model.counters |> List.mapi (fun index m -> view m (fun m -> dispatch (CounterMsg (index, m)))))
       | "test" -> yield R.str "test goes here"
       | "about" -> yield R.str "about goes here"
       | "configuration" -> yield R.str "configuration goes here"
       | _ -> yield R.str "should not happen"
    ]

let footer _ _ = 
  Footer.footer [] [
    R.str "footer goes here"
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
