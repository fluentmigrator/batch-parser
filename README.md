# SQL Batch Parser

This is a work-in-progress and **not** for production use.

## Intent

This project tries to solve the problem of finding (batches of) SQL statements
to solve problems like `GO` in a command in an embedded SQL script in the
FluentMigrator project.

## Structure

The main components are:

- `IRangeSearcher`
- `ISpecialTokenSearcher`

### `IRangeSearcher`

Search for a range of things - like comments, quoted identifiers and SQL
strings.

### `ISpecialTokenSearcher`

Search for special tokens like `GO`, or `;`. This allows
splitting the SQL script into SQL statements and batches.

## Example

```csharp
// This is the variable with your SQL script
var sqlScript = "...";

// This is the found SQL text
string sqlText = null;

// The parser does all the work for us
var parser = new SqlServerBatchParser();

// Handle the GO token
parser.SpecialToken += (sender, evt) => {
    // Handle the special token (e.g. GO)
    if (string.IsNullOrEmpty(sqlText)) {
        // A GO was found, but no SQL statements
        return;
    }

    // Execute the sqlText
    // TODO...

    // Reset the variable to avoid
    // executing the same SQL code
    // twice when a second GO follows
    // without SQL text in between.
    sqlText = null;
};

// Store the found SQL text
parser.SqlText += (sender, evt) => {
    sqlText = evt.SqlText.Trim();
};

// Define the source to be used by the parser
using (var source = new TextReaderSource(new StringReader(sqlScript), takeOwnership: true))
{
    // This is where the hard stuff happens
    parser.Process(source, stripComments: true);
}
```
