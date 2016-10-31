namespace FsharpTypeclasses

module Eq =
  type t<'a>

  val equality : t<'a> when 'a : equality

  module Ops = val eq : t<'a> -> ('a -> 'a -> bool)
  module Laws = val reflexivity : t<'a> -> 'a -> bool
