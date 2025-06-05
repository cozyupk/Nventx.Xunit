using NventX.xProof.BaseProofLibrary.Proofs;
using NventX.xProof.Xunit.SupportingBaseProofLibrary;
using Xunit;

namespace NventX.xProof.Xunit.E2ETests.ForXunitV2
{
    public class MyTestProof : FoolProof
    {
        public MyTestProof() : base()
        {
            // No additional initialization needed.
        }

#pragma warning disable CA1822 // Make member static
        public void ThrowException()
#pragma warning restore CA1822 // メンバーを static に設定します
        {
            throw new InvalidOperationException("This is a test exception from MyTestProof.");
        }
    }


    public class FoolProofActionTests
    {
        /*
        [ProofFact(typeof(MyTestProof))]
        public void TestMyProof(MyTestProof mtf)
        {
            mtf.ThrowException();
        }

        [ProofFact]
        public void FoolProof_CapturesExceptionFromActionWithLabel(FoolProof fp)
        {
            // Arrange
            bool actionExecuted = false;
            void throwingAction()
            {
                actionExecuted = true;
                throw new InvalidOperationException("Labeled failure");
            }

            // Act
            Exception immediateEx = Record.Exception(() => fp.Probe(throwingAction, "Custom Label"));

            // Assert (no exception thrown immediately, action did execute)
            Assert.Null(immediateEx);
            Assert.True(actionExecuted);

            // Assert that the failure is recorded with the label
            var failures = fp.CollectProbingFailure();
            Assert.Single(failures);
            Assert.Equal("Custom Label", failures.First().Message);
            Assert.IsType<InvalidOperationException>(failures.First().Exception);
        }

        [ProofTheory]
        public void SUCCESS_DoNothing(FoolProof fp)
        {
        }

        [ProofTheory(Skip="hoge")]
        public void SUCCESS_DoNothing2(FoolProof fp, int x)
        {
        }

        */

        /*
        [ProofTheory,
        InlineData(1, 2),
        InlineData(2),
        InlineData(3)]
        */
        [ProofFact]
        public void Fail_DoNothingInFact(FoolProof fp)
        {
            fp.Probe(() => throw new Exception($"(1) Failing with value in FoolProof."));
            throw new Exception($"(2) Failing with value in FoolProof.");
        }

        [ProofTheory,
            InlineData(345)]
        public void Fail_DoNothingInTheory(FoolProof fp, int x)
        {
            fp.Probe(() => throw new Exception($"(1) Failing with value {x} in FoolProof."));
            throw new Exception($"(2) Failing with value {x} in FoolProof.");
        }

        /*
        [Fact]
        public void FoolProo_CapturesExceptionFromSingleAction()
        {
            // Arrange
            bool actionExecuted = false;
            void throwingAction()
            {
                actionExecuted = true;
                throw new InvalidOperationException("Failing action");
            }
            var proof = new FoolProof();

            // Act
            Exception immediateEx = Record.Exception(() => proof.Probe(throwingAction));

            // Assert (no exception thrown immediately, action did execute)
            Assert.Null(immediateEx);
            Assert.True(actionExecuted);

            // Assert that the failure is recorded
            var failures = proof.CollectProbingFailure();
            Assert.Single(failures);
            Assert.IsType<InvalidOperationException>(failures.First().Exception);
            Assert.Contains("Failing action", failures.First().Exception.Message);
        }

        [Fact]
        public void FoolProof_CapturesExceptionsFromCombinedActions()
        {
            // Arrange
            bool firstActionExecuted = false;
            bool secondActionExecuted = false;
            void firstThrowingAction()
            {
                firstActionExecuted = true;
                throw new InvalidOperationException("First failure");
            }
            void secondThrowingAction()
            {
                secondActionExecuted = true;
                throw new ArgumentException("Second failure");
            }

            // Combine the two actions into one multicast delegate
            var proof = new FoolProof();
            var combinedAction = proof.Combine([firstThrowingAction, secondThrowingAction]);

            // Act
            Exception immediateEx = Record.Exception(() => combinedAction.Probe());

            // Assert (no exception thrown immediately, both actions executed)
            Assert.Null(immediateEx);
            Assert.True(firstActionExecuted);
            Assert.True(secondActionExecuted);

            // Assert that both failures are recorded
            var failures = proof.CollectProbingFailure();
            Assert.Equal(2, failures.Count());
            Assert.Contains("First failure", failures.First().Exception.Message);
            Assert.Contains("Second failure", failures.Last().Exception.Message);
        }
        */
    }

    /*
    public class FailLateProofTests
    {
        [Fact]
        public void FailLate_Should_NoRecordIfNoException()
        {
            var proof = new FailLateProof();
            proof.Setup(ProofInvocationKind.SingleCase);

            proof.FailLate(() => { });

            var failures = proof.CollectProbingFailure();
            Assert.Empty(failures);
        }

        [Fact]
        public void FailLate_Should_FailWithLabel()
        {
            var proof = new FailLateProof();
            proof.Setup(ProofInvocationKind.SingleCase);

            proof.FailLate(() => throw new InvalidOperationException("fail!"), "custom-label");

            var failure = Assert.Single(proof.CollectProbingFailure());
            Assert.Equal("custom-label", failure.Message);
            Assert.IsType<InvalidOperationException>(failure.Exception);
        }

        [Fact]
        // public void FailLate_Should_GenerateLabel_WhenNull()
        public void a()
        {
            var proof = new FailLateProof();
            proof.Setup(ProofInvocationKind.SingleCase);

            proof.FailLate(() => throw new Exception("fail"));

            var failure = Assert.Single(proof.CollectProbingFailure());
            Assert.Empty(failure.Message);
        }

        [Fact]
        public void FailLateParams_Should_NoRecordIfEmptyParams()
        {
            var proof = new FailLateProof();
            proof.Setup(ProofInvocationKind.SingleCase);

            proof.FailLate(new List<Action>().ToArray());

            var failures = proof.CollectProbingFailure();
            Assert.Empty(failures);
        }

        [Fact]
        public void FailLateParams_Should_NoRecordIfNoException()
        {
            var proof = new FailLateProof();
            proof.Setup(ProofInvocationKind.SingleCase);

            proof.FailLate([() => { }]);

            var failures = proof.CollectProbingFailure();
            Assert.Empty(failures);
        }

        [Fact]
        public void FailLateParams_Should_GenerateLabel()
        {
            throw new NotImplementedException("This test needs to be implemented.");
        }

        [Fact]
        public void FailLateParams_Should_RecordMultipleFailures()
        {
            throw new NotImplementedException("This test needs to be implemented.");
        }

        [Fact]
        public void FailLateWithReturnValue_Should_NoRecordIfNoException()
        {
            var proof = new FailLateProof();
            proof.Setup(ProofInvocationKind.SingleCase);

            proof.FailLate<string?>(() => null);

            var failures = proof.CollectProbingFailure();
            Assert.Empty(failures);
        }

        [Fact]
        public void FailLateWithReturnValue_Should_FailWithLabel()
        {
            var proof = new FailLateProof();
            proof.Setup(ProofInvocationKind.SingleCase);

            proof.FailLate<string>(() => throw new InvalidOperationException("fail!"), "custom-label");

            var failure = Assert.Single(proof.CollectProbingFailure());
            Assert.Equal("custom-label", failure.Message);
            Assert.IsType<InvalidOperationException>(failure.Exception);
        }

        [Fact]
        public void FailLateWithReturnValue_Should_GenerateLabel_WhenNull()
        {
            throw new NotImplementedException("This test needs to be implemented.");
        }

        [Fact]
        public void FailLateWithReturnValueParams_Should_NoRecordIfEmptyParams()
        {
            var proof = new FailLateProof();
            proof.Setup(ProofInvocationKind.SingleCase);

            proof.FailLate<string?>([]);

            var failures = proof.CollectProbingFailure();
            Assert.Empty(failures);
        }

        [Fact]
        public void FailLateWithReturnValueParams_Should_NoRecordIfNoException()
        {
            var proof = new FailLateProof();
            proof.Setup(ProofInvocationKind.SingleCase);

            proof.FailLate<string?>([() => null]);

            var failures = proof.CollectProbingFailure();
            Assert.Empty(failures);
        }

        [Fact]
        public void FailLateWithReturnValueParams_Should_GenerateLabel()
        {
            throw new NotImplementedException("This test needs to be implemented.");
        }

        [Fact]
        public void FailLateWithReturnValue_Should_RecordMultipleFailures()
        {
            throw new NotImplementedException("This test needs to be implemented.");
        }

        [Fact]
        public async Task FailLateAsync_Should_NoRecordIfNoException()
        {
            var proof = new FailLateProof();
            proof.Setup(ProofInvocationKind.SingleCase);

            await proof.FailLateAsync(() => Task.Run(() => { }));

            var failures = proof.CollectProbingFailure();
            Assert.Empty(failures);
        }

        [Fact]
        public async Task FailLateAsync_Should_FailWithLabel()
        {
            var proof = new FailLateProof();
            proof.Setup(ProofInvocationKind.SingleCase);

            await proof.FailLateAsync(() => Task.Run(() => { throw new InvalidOperationException("fail!"); }), "custom-label");

            var failure = Assert.Single(proof.CollectProbingFailure());
            Assert.Equal("custom-label", failure.Message);
            Assert.IsType<InvalidOperationException>(failure.Exception);
        }

        [Fact]
        public void FailLateAsync_Should_GenerateLabel_WhenNull()
        {
            throw new NotImplementedException("This test needs to be implemented.");
        }

        [Fact]
        public async Task FailLateAsyncParams_Should_NoRecordIfEmptyParams()
        {
            var proof = new FailLateProof();
            proof.Setup(ProofInvocationKind.SingleCase);

            await proof.FailLateAsync([]);

            var failures = proof.CollectProbingFailure();
            Assert.Empty(failures);
        }

        [Fact]
        public async Task FailLateAsyncParams_Should_NoRecordIfNoException()
        {
            var proof = new FailLateProof();
            proof.Setup(ProofInvocationKind.SingleCase);

            await proof.FailLateAsync([() => Task.Run(() => { })]);

            var failures = proof.CollectProbingFailure();
            Assert.Empty(failures);
        }

        [Fact]
        public void FailLateAsyncParams_Should_GenerateLabel()
        {
            throw new NotImplementedException("This test needs to be implemented.");
        }

        [Fact]
        public void FailLateAsync_Should_RecordMultipleFailures()
        {
            throw new NotImplementedException("This test needs to be implemented.");
        }
        */

    /*
    [Fact]
    public void Setup_Should_CompleteWithoutError()
    {
        var proof = new FailLateProof();
        proof.Setup(ProofInvocationKind.SingleCase);
        var failures = proof.CollectProbingFailure();
        Assert.Empty(failures);
    }

    [Fact]
    public void FailLate_Should_RecordException_WithLabel()
    {
        var proof = new FailLateProof();
        proof.Setup(ProofInvocationKind.SingleCase);

        proof.FailLate(() => throw new InvalidOperationException("fail!"), "custom-label");

        var failure = Assert.Single(proof.CollectProbingFailure());
        Assert.Equal("custom-label", failure.Message);
        Assert.IsType<InvalidOperationException>(failure.Exception);
    }

    [Fact]
    public void FailLate_Should_RecordMultipleFailures()
    {
        var proof = new FailLateProof();
        proof.Setup(ProofInvocationKind.SingleCase);

        proof.FailLate(
            () => throw new InvalidOperationException("fail1"),
            () => { },
            () => throw new ArgumentException("fail2")
        );

        var failures = proof.CollectProbingFailure().ToList();
        Assert.Equal(2, failures.Count);
        Assert.IsType<InvalidOperationException>(failures[0].Exception);
        Assert.IsType<ArgumentException>(failures[1].Exception);
    }

    [Fact]
    public void FailLate_Should_NoRecordIfNoException()
    {
        var proof = new FailLateProof();
        proof.Setup(ProofInvocationKind.SingleCase);

        proof.FailLate(() => { });
        proof.FailLate(() => Console.WriteLine("no error"));

        var failures = proof.CollectProbingFailure();
        Assert.Empty(failures);
    }

    [Fact]
    public void FailLate_Should_GenerateLabel_WhenNull()
    {
        var proof = new FailLateProof();
        proof.Setup(ProofInvocationKind.SingleCase);

        proof.FailLate(() => throw new Exception("fail"));

        var failure = Assert.Single(proof.CollectProbingFailure());
        Assert.False(string.IsNullOrWhiteSpace(failure.Message));
    }

    [Fact]
    public void FailLate_WithReturnValue_Should_ReturnExpectedResult_And_RecordNothing()
    {
        var proof = new FailLateProof();
        proof.Setup(default);

        int result = proof.FailLate(() => 10 + 5, "simple-add");
        Assert.Equal(15, result);

        var failures = proof.CollectProbingFailure();
        Assert.Empty(failures);
    }

    [Fact]
    public void FailLate_WithReturnValue_Should_ReturnDefault_OnException_And_RecordFailure()
    {
        var proof = new FailLateProof();
        proof.Setup(default);

        string? result = proof.FailLate<string>(() => throw new InvalidOperationException("oops"), "string-failure");
        Assert.Null(result);

        var failure = Assert.Single(proof.CollectProbingFailure());
        Assert.Equal("string-failure", failure.Message);
        Assert.IsType<InvalidOperationException>(failure.Exception);
    }


    [Fact]
    public async Task FailLateAsync_Should_RecordAsyncFailure()
    {
        var proof = new FailLateProof();
        proof.Setup(ProofInvocationKind.SingleCase);

        await proof.FailLateAsync(async () =>
        {
            await Task.Delay(10);
            throw new InvalidOperationException("async-fail");
        });

        var failure = Assert.Single(proof.CollectProbingFailure());
        Assert.Contains("async", failure.Exception.Message);
    }

    [Fact]
    public async Task FailLateAsync_Should_RecordMultipleAsyncFailures()
    {
        var proof = new FailLateProof();
        proof.Setup(ProofInvocationKind.SingleCase);

        await proof.FailLateAsync(
            async () =>
            {
                await Task.Delay(10);
                throw new InvalidOperationException("a");
            },
            async () => await Task.CompletedTask,
            async () =>
            {
                await Task.Yield();
                throw new ArgumentNullException("b");
            }
        );

        var failures = proof.CollectProbingFailure().ToList();
        Assert.Equal(2, failures.Count);
        Assert.IsType<InvalidOperationException>(failures[0].Exception);
        Assert.IsType<ArgumentNullException>(failures[1].Exception);
    }

    [Fact]
    public void FailLateAsync_Should_RecordNoFailure_WhenNoException()
    {
    }

    [Fact]
    public void FailLate_ParamsFunc_Should_CollectAllResults_AndRecordFailures()
    {
        var proof = new FailLateProof();
        proof.Setup(default);

        var results = proof.FailLate(
            () => "alpha",
            () => throw new Exception("fail1"),
            () => "beta",
            () => throw new InvalidOperationException("fail2")
        ).ToArray();

        Assert.Equal(new string?[] { "alpha", null, "beta", null }, results);

        var failures = proof.CollectProbingFailure().ToList();
        Assert.Equal(2, failures.Count);
        Assert.Contains("fail1", failures[0].Exception.Message);
        Assert.Contains("fail2", failures[1].Exception.Message);
    }

    [Fact]
    public void FailLate_ParamsFunc_Should_RecordDefaultLabel_WhenMissing()
    {
        var proof = new FailLateProof();
        proof.Setup(default);

        var results = proof.FailLate<string>(
            [() => throw new Exception("unnamed fail")]
        ).ToArray();

        var failure = Assert.Single(proof.CollectProbingFailure());
        Assert.False(string.IsNullOrWhiteSpace(failure.Message));
        Assert.Contains("unnamed fail", failure.Exception.Message);
        Assert.Null(results[0]);
    }

    [Fact]
    public void FailLate_ParamsFunc_Should_RecordNoFailure_WhenNoException()
    {
        var proof = new FailLateProof();
        proof.Setup(default);

        var results = proof.FailLate(
            () => 1,
            () => 2,
            () => 3
        ).ToArray();

        Assert.Equal([1, 2, 3], results.ToArray());
        Assert.Empty(proof.CollectProbingFailure());
    }

    [Fact]
    public void CollectProbingFailure_Should_ReturnInOrder()
    {
        var proof = new FailLateProof();
        proof.Setup(ProofInvocationKind.SingleCase);

        proof.FailLate(() => throw new Exception("1"), "a");
        proof.FailLate(() => throw new Exception("2"), "b");

        var failures = proof.CollectProbingFailure().ToList();
        Assert.Equal("a", failures[0].Message);
        Assert.Equal("b", failures[1].Message);
    }
}
    */
}
