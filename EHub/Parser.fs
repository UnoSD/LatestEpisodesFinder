module Parser

open CommandLine
open System.Globalization
open CommandLineOptions
open System

type CommandLineOption = 
      Find of FindOptions
    | Scan of ScanOptions
    | Error of string list

let parseArguments args =
    let settings (settings : ParserSettings) =
        settings.ParsingCulture <- CultureInfo.CurrentCulture;
        settings.IgnoreUnknownArguments <- false

    let settingsAction =
        Action<ParserSettings>(settings)

    use parser =
        new Parser(settingsAction) 

    let parseResult =
        args |> parser.ParseArguments<FindOptions, ScanOptions>

    let toOption (result : obj) =
        match result with
        | :? FindOptions as o -> Find o
        | :? ScanOptions as o -> Scan o
        | _                   -> Error [ "Invalid verb" ]
        
    match parseResult with
    | :? Parsed<obj>    as parsed    -> toOption parsed.Value
    | :? NotParsed<obj> as notParsed -> notParsed.Errors |>
                                        Seq.map (fun e -> e.ToString()) |>
                                        Seq.toList |>
                                        Error
    | _                              -> [ "Invalid parser result." ] |> Error


                   