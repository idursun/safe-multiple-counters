module App

open Elmish
open Elmish.React
open Counter

open Fable.Helpers.React.Props
module R = Fable.Helpers.React


type Model = {
    counters: Counter.Model list
  }

type Msg =
  | CounterMsg of int * Counter.Msg

let init () = { counters = [Counter.init (); Counter.init ()] }

let update msg (model : Model) =
  match msg with
  | CounterMsg (index, m) -> { model with counters = model.counters |> List.mapi (fun i c -> if i = index then (Counter.update m c) else c) }

let view model dispatch =
  let combined i m = CounterMsg (i, m) |> dispatch
  model.counters |> List.mapi (fun index m -> Counter.view m (combined index)) |> R.div []

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkSimple init update view
#if DEBUG
|> Program.withConsoleTrace
|> Program.withHMR
#endif
|> Program.withReact "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
