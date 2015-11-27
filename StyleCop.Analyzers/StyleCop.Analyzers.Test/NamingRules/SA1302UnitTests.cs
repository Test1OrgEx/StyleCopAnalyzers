﻿// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.NamingRules
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.Diagnostics;
    using StyleCop.Analyzers.NamingRules;
    using TestHelper;
    using Xunit;

    public class SA1302UnitTests : CodeFixVerifier
    {
        [Fact]
        public async Task TestInterfaceDeclarationDoesNotStartWithIAsync()
        {
            var testCode = @"
public interface Foo
{
}";

            DiagnosticResult expected = this.CSharpDiagnostic().WithLocation(2, 18);

            await this.VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(false);

            var fixedCode = @"
public interface IFoo
{
}";

            await this.VerifyCSharpFixAsync(testCode, fixedCode).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestInterfaceDeclarationDoesNotStartWithIPlusInterfaceUsedAsync()
        {
            var testCode = @"
public interface Foo
{
}
public class Bar : Foo
{
}";

            DiagnosticResult expected = this.CSharpDiagnostic().WithLocation(2, 18);

            await this.VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(false);

            var fixedCode = @"
public interface IFoo
{
}
public class Bar : IFoo
{
}";

            await this.VerifyCSharpFixAsync(testCode, fixedCode).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestInterfaceDeclarationStartsWithLowerIAsync()
        {
            var testCode = @"
public interface iFoo
{
}";

            DiagnosticResult expected = this.CSharpDiagnostic().WithLocation(2, 18);

            await this.VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(false);

            var fixedCode = @"
public interface IiFoo
{
}";

            await this.VerifyCSharpFixAsync(testCode, fixedCode).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestInnerInterfaceDeclarationDoesNotStartWithIAsync()
        {
            var testCode = @"
public class Bar
{
    public interface Foo
    {
    }
}";

            DiagnosticResult expected = this.CSharpDiagnostic().WithLocation(4, 22);

            await this.VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestInterfaceDeclarationDoesStartWithIAsync()
        {
            var testCode = @"public interface IFoo
{
}";

            await this.VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestInnerInterfaceDeclarationDoesStartWithIAsync()
        {
            var testCode = @"
public class Bar
{
    public interface IFoo
    {
    }
}";

            await this.VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestComInterfaceInNativeMethodsClassAsync()
        {
            var testCode = @"
using System.Runtime.InteropServices;
public class NativeMethods
{
    [ComImport, Guid(""C8123315-D374-4DB8-9E7A-CB3499E46F2C"")]
    public interface FileOpenDialog
    {
    }
}";

            await this.VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestComInterfaceInNativeMethodsClassWithIncorrectNameAsync()
        {
            var testCode = @"
using System.Runtime.InteropServices;
public class NativeMethodsClass
{
    [ComImport, Guid(""C8123315-D374-4DB8-9E7A-CB3499E46F2C"")]
    public interface FileOpenDialog
    {
    }
}";

            DiagnosticResult expected = this.CSharpDiagnostic().WithLocation(6, 22);

            await this.VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(false);

            var fixedCode = @"
using System.Runtime.InteropServices;
public class NativeMethodsClass
{
    [ComImport, Guid(""C8123315-D374-4DB8-9E7A-CB3499E46F2C"")]
    public interface IFileOpenDialog
    {
    }
}";

            await this.VerifyCSharpFixAsync(testCode, fixedCode).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestComInterfaceInInnerClassInNativeMethodsClassAsync()
        {
            var testCode = @"
using System.Runtime.InteropServices;
public class MyNativeMethods
{
    public class FileOperations
    {
        [ComImport, Guid(""C8123315-D374-4DB8-9E7A-CB3499E46F2C"")]
        public interface FileOpenDialog111
        {
        }
    }
}";

            await this.VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestInterfaceDeclarationDoesNotStartWithIWithConflictAsync()
        {
            string testCode = @"
public interface Foo
{
}

public interface IFoo { }";
            string fixedCode = @"
public interface IFoo1
{
}

public interface IFoo { }";

            DiagnosticResult expected = this.CSharpDiagnostic().WithLocation(2, 18);

            await this.VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(false);
            await this.VerifyCSharpDiagnosticAsync(fixedCode, EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
            await this.VerifyCSharpFixAsync(testCode, fixedCode, cancellationToken: CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestInterfaceDeclarationDoesNotStartWithIWithMemberConflictAsync()
        {
            string testCode = @"
public interface Foo
{
    int IFoo { get; }
}";
            string fixedCode = @"
public interface IFoo1
{
    int IFoo { get; }
}";

            DiagnosticResult expected = this.CSharpDiagnostic().WithLocation(2, 18);

            await this.VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(false);
            await this.VerifyCSharpDiagnosticAsync(fixedCode, EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
            await this.VerifyCSharpFixAsync(testCode, fixedCode, cancellationToken: CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestNestedInterfaceDeclarationDoesNotStartWithIWithConflictAsync()
        {
            string testCode = @"
public class Outer
{
    public interface Foo
    {
    }

    public interface IFoo { }
}";
            string fixedCode = @"
public class Outer
{
    public interface IFoo1
    {
    }

    public interface IFoo { }
}";

            DiagnosticResult expected = this.CSharpDiagnostic().WithLocation(4, 22);

            await this.VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(false);
            await this.VerifyCSharpDiagnosticAsync(fixedCode, EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
            await this.VerifyCSharpFixAsync(testCode, fixedCode, cancellationToken: CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestNestedInterfaceDeclarationDoesNotStartWithIWithContainingTypeConflictAsync()
        {
            string testCode = @"
public class IFoo
{
    public interface Foo
    {
    }
}";
            string fixedCode = @"
public class IFoo
{
    public interface IFoo1
    {
    }
}";

            DiagnosticResult expected = this.CSharpDiagnostic().WithLocation(4, 22);

            await this.VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(false);
            await this.VerifyCSharpDiagnosticAsync(fixedCode, EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
            await this.VerifyCSharpFixAsync(testCode, fixedCode, cancellationToken: CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestNestedInterfaceDeclarationDoesNotStartWithIWithNonInterfaceConflictAsync()
        {
            string testCode = @"
public class Outer
{
    public interface Foo
    {
    }

    private int IFoo => 0;
}";
            string fixedCode = @"
public class Outer
{
    public interface IFoo1
    {
    }

    private int IFoo => 0;
}";

            DiagnosticResult expected = this.CSharpDiagnostic().WithLocation(4, 22);

            await this.VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(false);
            await this.VerifyCSharpDiagnosticAsync(fixedCode, EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
            await this.VerifyCSharpFixAsync(testCode, fixedCode, cancellationToken: CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestInterfaceDeclarationDoesNotStartWithIWithConflictInAnotherAssemblyAsync()
        {
            string testCode = @"
namespace System
{
    public interface Disposable
    {
    }
}
";
            string fixedCode = @"
namespace System
{
    public interface IDisposable1
    {
    }
}
";

            DiagnosticResult expected = this.CSharpDiagnostic().WithLocation(4, 22);

            await this.VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(false);
            await this.VerifyCSharpDiagnosticAsync(fixedCode, EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
            await this.VerifyCSharpFixAsync(testCode, fixedCode, cancellationToken: CancellationToken.None).ConfigureAwait(false);
        }

        protected override IEnumerable<DiagnosticAnalyzer> GetCSharpDiagnosticAnalyzers()
        {
            yield return new SA1302InterfaceNamesMustBeginWithI();
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new SA1302CodeFixProvider();
        }
    }
}