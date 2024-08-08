import random

suits = ["hearts", "diamonds", "spades", "clubs"]

x = random.random(1,13)
xs = random.random(1,4)
y = random.random(1,13)
ys = random.random(1,4)
print("your cards are the " + x + " of " + suits[xs] + " and the " + y + " of " + suits[ys])