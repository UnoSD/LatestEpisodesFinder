open ResultBuilder
open Parser

[<EntryPoint>]
let main args =
    let result =
        result {
                   let! parsed =
                       match args |> Array.toList with
                       | "find"::tail -> Ok (parseArguments tail)
                       | _            -> Result.Error [ "Unknown verb." ]

                   do 
                    match parsed with
                    | Find  f -> printf "%A" f
                    | Scan  f -> printf "%A" f
                    | Error e -> printf "%A" e

                   return Error [ ]
               }
    
    let b =
        match result with
        | Ok x -> x
        | _ -> CommandLineOption.Error [ ]

    0