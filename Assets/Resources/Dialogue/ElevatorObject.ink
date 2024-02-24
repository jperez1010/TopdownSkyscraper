EXTERNAL useElevator()

->UseElevator

==UseElevator==
-Would you like to leave base?
    + Exit
    ->GoToADifferentFloor
    + No
    ->DoNotUseElevator

==GoToADifferentFloor==
~useElevator()
->END

==DoNotUseElevator==
- Come back when you're ready to leave.
-> END