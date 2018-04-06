﻿#region License
// Copyright (c) 2018, Fluent Migrator Project
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

namespace FluentMigrator.BatchParser.RangeSearchers
{
    /// <summary>
    /// An SQL server style quoted identifer (<c>[identifier]</c>)
    /// </summary>
    public sealed class SqlServerIdentifier : CharWithEscapeByDuplication
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerIdentifier"/> class.
        /// </summary>
        public SqlServerIdentifier()
            : base('[', ']')
        {
        }
    }
}
