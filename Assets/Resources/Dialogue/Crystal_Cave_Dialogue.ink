INCLUDE Globals.ink

{Illinois_James == "": -> IllinoisIntro}
{Illinois_James == "FirstEncounter": -> FirstEncounter}
{Illinois_James == Waiting: -> Idle}

== IllinoisIntro ==
-Hmm... let's see, I don't remember mapping a pool of water...
-!!!
-Phew... you scared me. 
-I thought you might be some mythomorph that creeped up on me. 
-According to my notes, there shouldn't being an entrance to the cave around here, though. 
-How'd you manage your your way in here, kid?

 * There was a large hole in the ground so naturally I jumped in.
 
 -Ahh, a cave-in. That would explain all this uncharted water! 
 -The ground collapsing must have uncovered an underground stream. 
 -That'll surely make getting out of here more troublesome than it already was...
 -!!!
 -Oh sorry, I guess I should introduce myself.
 -I'm Illinois James:
 -Explorer, researcher, mythtorian and professional super model. And you are?
 
 + Wait, professional what?
    -> Model
 + My name is Marco, and I'm going to become the strongest myth master!
    -> MythMaster

== Model ==
    - Most people are caught off guard when I ask that, 
    - Which is why I always come prepared. 
    - Here's my photo on page 8 of the annual mythtorian calendar.
    * I didn't need to see that
    - Well, you're the one who asked, anyways, what's your name?
    * My name is Marco, and I'm going to become the strongest myth master.
    -> MythMaster

== MythMaster ==
    - The strongest myth master, eh? 
    - You would get along swimmingly with my intern. 
    - Welp, that's a nice goal and all but so long as were stuck down here...
    - the only thing I see us becoming is a plaything for a wild morph. 
    * Well, how did you get in here in the first place?
    - I came through an entrance connected to some ruins I've been investigating. 
    - Unfortunately, I was chased out by a swarm of mythomorphs.
    - Must have been caused by your little cave-in.
    - Even worse, it seems I lost my mythogem during my escape...
    - !!!
    - Hold on a second, I see you have a mythogem wrapped around your neck!
    - You wouldn't happen to have any mythomorphs at your disposal now would you?
    * Well, it would be a bit hard to be a myth master and all without one.
    - ... 
    - Alright. If I'm able to get us to where I dropped my mythogem...
    - I can use my morphs to guide us to the ruins entrance.
    - But to do that, we would have to fend off any wild morphs that try to stop us on the way there.
    - I feel bad asking this, but since it's our only choice;
    - Marco, would you be able to clear the way for us to get there?
    * Sure, sounds like good practice. Just stay behind me, old man, I'll keep you safe.
    -I'm 30
    * Yikes
    - Maybe being stuck here all alone wasn't such a bad thing.
    ~ Illinois_James = Waiting
    ->DONE
    
== Idle ==
- What are you waiting for!?
- Let's start heading out of here!
->DONE

== FirstEncounter ==
- !!!
- Well, looks like our luck has already ran short. 
- That's a sharpuch, a nasty little critter that will hunt you down relentlessly in the hopes of stealing any shinies you may have on you.
- It looks like we'll have to fight him in order to get past. Alright, go on and show him what you got, kid!
+ ... How do I fight him?
    -> Tutorial
+ This'll only take a second.
    -> DONE

== Tutorial==
 - ... You mean to tell me you've NEVER fought a mythomorph before?
 * That's right
 - But you said you were going to be the strongest Myth Master?!!
 * Yeah, key words "GOING TO BE"..
 - What th-
 - Sigh
 - Alright, fine. I may not be an expert, but I can give you a few pointers to start out.
 - In mythomorph battles, you got two types of basic attacks: LIGHT Attacks and HEAVY attacks. 
 - LIGHT attacks are moves that can be thrown out at a moments notice, with little delay in between each one.
 - These can be used by pressing right click.
 - HEAVY attacks are moves that take a little longer to charge up and require a small break in between each.
 - In return, they dish out much more damage than a LIGHT attack. 
 - These can be used by pressing left click
 * Seems simple enough.
 - Alright, why not try your skills out on Sharpuch, then, see what you got?
 * This'll only take a second.


-> DONE

== Fight_Done ==
- Well shoot...
- I was a bit worried there but nicely done!
* Told you it be easy.
- Well let's hope it stays that way. Come on! Let's keep going.
-> DONE

== Key ==
- Hmm.. this door wasn't closed when I went through it.
* Well, why don't we just open it?
- We need a key to open the door, but it seems to have gone missing, probably snatched by
    one of the critters lurking about.
- Look around the area and see if you can find the key. Stay on your toes though, we don't know what's hiding in the dark.
* Yeah, hopefully it's strong mythomorphs.
- Kid, you're making this a lot more stressful than it already is.
-> DONE

== Key_Obtained ==
- Great job! Get inside and let's get to the ruin.
-> DONE

== Ruin_Entrance ==
Alright, we're almost to where I was when I lost my mythogem. It should be just a bit further down. Let's hurry before we run into any serious trouble.
-> DONE

== Boss_Encounter ==
- Well, you wanted a challenge kid, you got one. Good luck.
* Piece of cake
->DONE

== Temple_End_Arrival ==
- Phew, looks like we arrived in one piece. I had my doubts but I gotta hand it to you, kid, you really did pull through. Give me a second to look around for my mythogem and we'll be on our way.
-> DONE

== Exit ==
- Follow me! We're getting out of here!
-> DONE