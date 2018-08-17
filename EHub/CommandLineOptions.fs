module CommandLineOptions

open CommandLine

[<Verb("find", HelpText = "Find media in the database.")>]
type FindOptions = 
    {
        [<Value(0, (*Min=0, *)HelpText = "Input the name of the media to find.")>]
        searchTerms : string seq
    }

type FindVerbOptions = 
    {
        [<Value(0, Default="all")>]
        what : string
    }

[<Verb("list", HelpText = "List all media in the database.")>]
type ScanOptions = 
    {
        [<Option>]
        path : string
    }