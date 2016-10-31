namespace FsharpTypeclasses

module Functor =
  type t<'a, 'b, 'fa, 'fb>

  val option : t<'a, 'b, option<'a>, option<'b>>
  val list : t<'a, 'b, list<'a>, list<'b>>
  val array : t<'a, 'b, array<'a>, array<'b>>
  val tuple2 : t<'a, 'b, 'a * 'a, 'b * 'b>

  module Ops =
    val map : t<'a, 'b, 'fa, 'fb> -> (('a -> 'b) -> 'fa -> 'fb)

  module Laws =
    val identity : t<'a, 'a, 'fa, 'fa> -> Eq.t<'fa> -> 'fa -> bool
    val composition :
      t<'a, 'b, 'fa, 'fb> ->
      t<'b, 'c, 'fb, 'fc> ->
      t<'a, 'c, 'fa, 'fc> ->
      Eq.t<'fc> ->
      ('a -> 'b) ->
      ('b -> 'c) ->
      'fa ->
      bool
