# SandTNTRatioFinder
Brute Force equal ratio for sand and tnt in newer minecraft versions.
(This program is WIP, please report any issues or bugs!)

## How to use:
1) Download Release or Build with Visual Studio
2) Open SandTNTRatioFinderPublic.exe.config
3) Change Settings and Save
4) Run SandTNTRatioFinderPublic.exe

## Configuration:
maxgametickdifference - The highes gametick difference of your sand and tnt booster. (Higher difference will mean possibly higher difference in velocity in the barrel.)

maxgametickdrop - How long the sand and tnt is allowed to drop in the barrel. (This starts after the tnt got boosted so the maximum time the sand can drop can be higher.)

differencethreshhold - How close in y distance the exposure points of the tnt and sand need to be in order to save the ratio.

showratios - How many ratios to show after the calculation completes. (Sorted from best to worst.)

maxguiderydifference - How far the guiders can be apart in y level.

verbose - Enable advanced output while calculating (This will severely slow down the program.)

savetofile - Save the ratios found to the ratios.txt file in the same folder as the program.

## Understanding the Information:
Example Ratio:
Sand Boost GT: 0 | TNT Boost GT: 1
(This means the tnt booster needs to go off 1gt after the sand booster.)

Sand Guider: sideways_skull | TNT Guider: top_medium_amethyst
(These are the top alignment guiders for both sand and tnt.)

Sand Guider Y: 0 | TNT Guider Y: 0
(This is the y level of the guiders, in this case they would be on the same level but if TNT Guider Y was 1 it would be 1 block higher and if it was -1 it would be 1 block lower.)

Gameticks Dropped: 9 | Effective Difference: 0.000495524260299529
(Gameticks Dropped means after how many gt in the barrel the difference will be smallest. Effective difference is the difference the exposure points should have.)

Sand Pos: -0.604280688754689 (-1.43728068875469) | TNT Pos: -0.604776213014989 (-0.604776213014989)
(The first position shows is the exposure position, the one in brackets is the real position.)

Sand Y Vel: -0.325854386224906 | TNT Y Vel: -0.2925044757397
(This shows the Y velocity of the entities at the time they have the difference. Might be needed for calculating your power.)

## Additional Information:
- TNT gets Exposure applied at its y position (0 up in the block) while sand gets it applied at its eye position (0.833 up in the block). The explosions happens 1 pixel up in the tnt (0.06125 up in the block).
- When using cannondebug to view the ratios you can look for the TNT Y Velocity to easily find the right tick.
- The program does not know how you are setting up your guiders, make sure you have enough booster and set up the guiders correctly.

## Thanks
Thanks to BelgianBomber, SplitBlade, Scobur and timur_the_mogol for helping me with different things as well as keeping me motivated while making this.
