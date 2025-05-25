using System;
using NventX.FoundationPkg.Abstractions.Observing;
using Xunit;

namespace NventX.FoundationPkg.UnitTests.Traits
{
    public class ConcurrentSelfCleaningSet
    {
        /*
        public class ExceptionCases : TheoryData<string, Action, Type, string>
        {
            public ExceptionCases()
            {
                AddRows([
                    [
                        "AddElementsInvokedWithNullParameter",
                        () => {
                            var hashSet = new ConcurrentSelfCleaningSet<string>();
                            hashSet.AddElements(null!);
                        },
                        typeof(ArgumentNullException),
                        "elements cannot be null."
                    ],
                    [
                        "AddElementsInvokedWithAListContaingNullValue",
                        () => {
                            var hashSet = new ConcurrentSelfCleaningSet<string>();
                            hashSet.AddElements("test", null!);
                        },
                        typeof(ArgumentNullException),
                        "elements contains a null element."
                    ],
                ]);
            }
        }

        [Theory]
        [ClassData(typeof(ExceptionCases))]
        public void ExceptionCases_Throws_ExpectedException(string _, Action act, Type ExceptionType, string expectedMessage)
        {
            // Arrange &  Act
            var ex = Record.Exception(() => act());

            // Assert
            Assert.NotNull(ex);
            Assert.IsType(ExceptionType, ex);
            Assert.Contains(expectedMessage, ex.Message);
        }
        */
    }
}
