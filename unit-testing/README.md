# Unit Testing

The aim is to learn Unit testing.

## Structure

We put the unit test project under the `test` folder.

## Test Case

In our test cases, the objective is to offer just enough information to
comprehend the target scenario. We conceal irrelevant details using helper
methods or omit them altogether, as they do not contribute to the comprehension
of the case. Furthermore, we standardize our test cases using the
[3A Pattern](/README.md#3a-pattern).

### 3A Pattern

This approach gives us consistency and clarity across the tests by addressing
the aspects of Arrange, Act, and Assert in a structured manner.

### Test Case Naming

Test case names should be written as sentences in a descriptive manner,
providing a clear idea of the case's purpose, including the given input
and expected output. The primary objective is to understand the
functionality of the module being tested solely from the test case names,
without revealing the underlying structure.

## Assertions Library

We utilize the `Shouldly` library as our chosen assertion library due to its
alignment with the project's established structure and extensions. Its
implementation minimizes the effort required to modify tests whenever changes
are introduced.

## Mock Library

TBD...

## GiveMe & MockMe Usage

In our tests, we refactor the redundant information and helper methods,
consolidating them into extension methods. We create a source class called
`Stubber` to invoke these extension methods. This approach allows us to
centralize the helper methods in one place.

By using the name `GiveMe` for these methods, we ensure meaningful and
descriptive naming in our test cases.

`MockMe` TBD...
