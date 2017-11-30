module Counter

open Fable.Helpers.React.Props
module R = Fable.Helpers.React

type Model = int

type Msg = Increment | Decrement

let init () = 0

let update msg model =
    match msg  with
    |Increment -> model + 1
    |Decrement -> model - 1


let view model dispatch =
  R.div []
      [ R.button [ OnClick (fun _ -> dispatch Decrement) ] [ R.str "-" ]
        R.div [] [ R.str (sprintf "%A" model) ]
        R.button [ OnClick (fun _ -> dispatch Increment) ] [ R.str "+" ] ]
