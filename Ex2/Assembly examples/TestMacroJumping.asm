@0
M=0

//test JMP
@1
D=A
D;JMP:JMP1
@0
M=M+1
(JMP1)


//test JEQ
@0
D=A
D;JEQ:JMP2
@0
M=M+1
(JMP2)


//test JLT
@1
D=0
D=D-A
D;JMP:JMP3
@0
M=M+1
(JMP3)


//test JGT
@1
D=A
D;JGT:JMP4
@0
M=M+1
(JMP4)

