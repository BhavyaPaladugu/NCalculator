# Project name:
NCalculatorConsoleApplication

# Description:
This application will allow users to perform single and batch calculations for n numbers.

# Internal Design:
Leveraged Interface implementation for future extension of application without disturbing main components. Created Interface Icalculator with abstract method calculate which takes as input a string. Created Calculator class that implements Icalculator.

Icalculator.cs: Icalculator interface has two abstract methods 

                    1.  Int Calculate ();
                      Purpose: To calculate the input value.
                      
                    2.  bool ValidateInput(string input);
                      Purpose: To validate the input string to accept only (+,-,* and /) operators and {0 to 9} digits.
                      
Caclulator.cs: Parses input and stores digits and operators in stack with respect to their precedence and compiles them using Expressions.

Operation.cs: It’s like a helper class. We can add more operators like unary by extending operator class and is very flexible with n numbers and operators in input.

#Assumptions:
No unary operators or parentheses passed in input.
