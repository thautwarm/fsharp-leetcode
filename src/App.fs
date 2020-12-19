module App
open Fable.Core
open Fable.Core.JsInterop

type minstack<'t when 't : comparison>() =
  let data = System.Collections.Generic.List<'t>()
  let mutable minE = set [] : Set<'t * int>
  member __.push x =
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


let createMinStack() = minstack()
let push(st: minstack<'t>, x) = st.push x
let pop(st: minstack<'t>) = st.pop()
let top(st: minstack<'t>) = st.top()
let getMin(st: minstack<'t>) = st.getMin()



#if NETCOREAPP
[<EntryPoint>]
let main _ =
  let a = minstack()
  a.push(-2)
  a.push(0)
  a.push(-3)
  printfn "%A" <| a.getMin()
  a.pop()
  printfn "%A" <| a.top()
  printfn "%A" <| a.getMin()
  0

#else
emitJsStatement () "
  export class MinStack{
    constructor(){
      this.inner = createMinStack()
    }
    push(a){ push(this.inner, a) }
    pop(){ pop(this.inner) }
    top(){ return top(this.inner) }
    getMin(){ return getMin(this.inner) }
  }
"
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