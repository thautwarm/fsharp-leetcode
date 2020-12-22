module App
open Fable.Core
open Fable.Core.JsInterop

    
[<Fable.Core.AttachMembers>]
[<CompiledName("MinStack")>]
type MinStack<'t when 't : comparison>() =
  let data = System.Collections.Generic.List<'t>()
  let mutable minE = set [] : Set<'t * int>
  member __.push(x) =
    data.Add x
    minE <- Set.add (x, data.Count - 1) minE
  member __.pop () =
    let i = data.Count - 1
    let x = data.[i]
    minE <- Set.remove (x, i) minE
    data.RemoveAt i
  member __.top () = data.[data.Count - 1]
  member __.getMin(): 't =
    let x, _ = Set.minElement minE in x

#if NETCOREAPP
[<EntryPoint>]
let main _ =
  let a = MinStack()
  a.push(-2)
  a.push(0)
  a.push(-3)
  printfn "%A" <| a.getMin()
  a.pop()
  printfn "%A" <| a.top()
  printfn "%A" <| a.getMin()
  0

#else
#if DEBUG
emitJsStatement () "
  (function(a){
    a.push(-2)
    a.push(0)
    a.push(-3)
    console.log(a.getMin())
    a.pop()
    console.log(a.top())
    console.log(a.getMin())
  })(new MinStack());


  (function(a){
    a.push(2)
    a.push(0)
    a.push(3)
    console.log(a.getMin())
    a.pop()
    console.log(a.top())
    console.log(a.getMin())
  })(new MinStack());
"
#endif
#endif