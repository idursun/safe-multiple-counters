
module Components.Header

open Elmish
open Elmish.React

open Fable.Helpers.React.Props
module R = Fable.Helpers.React

open Fulma
open Fulma.Components
open Fulma.Layouts

type Msg = GoPage of string

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
