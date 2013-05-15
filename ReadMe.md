Creates a string of a number of words, each 10 chars long separated by a .
the each routine is called and the result itterated 10000 times to see how it does

@10000 words

    av 00:00:00.0006927, 10000x indexOf (yield)
    av 00:00:00.0010268, 10000x string.split
    av 00:00:00.0014961, 10000x indexOf
    av 00:00:00.0022988, 10000x loop
    av 00:00:00.0024490, 10000x loop (yield)
    av 00:00:00.0049712, 10000x regex.split (compiled)
    av 00:00:00.0059776, 10000x regex.split

@1000 words

    av 00:00:00.0000697, 10000x indexOf (yield)
    av 00:00:00.0000786, 10000x indexOf
    av 00:00:00.0000799, 10000x string.split
    av 00:00:00.0001607, 10000x loop
    av 00:00:00.0002471, 10000x loop (yield)
    av 00:00:00.0002789, 10000x regex.split (compiled)
    av 00:00:00.0003892, 10000x regex.split
