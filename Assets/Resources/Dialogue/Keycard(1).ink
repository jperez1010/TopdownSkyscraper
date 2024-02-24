EXTERNAL openDoor()

VAR key1= false
VAR key = ""

{key == "true": -> GasConfirmed}
{key == "false": -> Gas}


==GasConfirmed==
~openDoor()
Smells

->END
==Gas==
(Did you do the gassy)
-> END

