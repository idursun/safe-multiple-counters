module App

open Elmish
open Elmish.React
open Counter

open Fable.Helpers.React.Props
module R = Fable.Helpers.React

open Fable.PowerPack.Fetch


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
                      let _updatedCounters =  model.counters |> List.mapi(fun i c-> if  i = index then fst (Counter.update m c) else c)
                      { model with counters = _updatedCounters}, Cmd.none
  model', cmd

let view model dispatch =
  let combined i m = CounterMsg (i, m) |> dispatch
  model.counters |> List.mapi (fun index m -> view m (combined index)) |> R.div []

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
