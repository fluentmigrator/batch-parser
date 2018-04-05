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
