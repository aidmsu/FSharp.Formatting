﻿#if INTERACTIVE
#r "../../bin/FSharp.Markdown.dll"
#r "../../packages/NUnit.2.6.3/lib/nunit.framework.dll"
#load "../Common/FsUnit.fs"
#else
module FSharp.Markdown.Tests.Parsing
#endif

open FsUnit
open NUnit.Framework
open FSharp.Markdown

[<Test>]
let ``Headings ending with F# are parsed correctly`` () =
  let doc = """
## Hello F#
Some more""" |> Markdown.Parse

  doc.Paragraphs
  |> shouldEqual [
      Heading(2, [Literal "Hello F#"]); 
      Paragraph [Literal "Some more"]]

[<Test>]
let ``Headings ending with spaces followed by # are parsed correctly`` () =
  let doc = """
## Hello ####
Some more""" |> Markdown.Parse

  doc.Paragraphs
  |> shouldEqual [
      Heading(2, [Literal "Hello"]); 
      Paragraph [Literal "Some more"]]

[<Test>]
let ``Can escape special characters such as "*" in emphasis`` () =
  let doc = """*foo\*\*bar*""" |> Markdown.Parse
  let expected = Paragraph [Emphasis [Literal "foo**bar"]] 
  doc.Paragraphs.Head 
  |> shouldEqual expected
      
[<Test>]
let ``Inline code can contain backticks when wrapped with spaces`` () =
  let doc = """` ``h`` `""" |> Markdown.Parse
  let expected = Paragraph [InlineCode "``h``"]
  doc.Paragraphs.Head 
  |> shouldEqual expected
