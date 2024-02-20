EXTERNAL saveGame()

->SaveGame

==SaveGame==
-Would you like to save your progress?
    + Yes
    ->SaveProgress
    + No
    ->DoNotSaveProgress

==SaveProgress==
~saveGame()
-Progress has been saved.
->END

==DoNotSaveProgress==
-Progress has not been saved.
-> END