// Computes R2 = R0^R1

//check if base=0:
@0
D=M
@Z_BASE
D;JEQ

//check if base=1:
D=D-1
@ONE_BASE
D;JEQ

//check if power=0:
@1
D=M
@Z_POW
D;JEQ

//check if pow=1:
D=D-1
@ONE_POW
D;JEQ

//otherwise:
@0
D=M
@temp
M=D
@2
M=D
D=A
@i
M=D
(LOOP1)
@2
D=A
@j
M=D
(LOOP2)
@2
D=M
@temp
M=D+M
@0
D=M
@j
D=D-M
M=M+1
@LOOP2
D;JGT
@temp
D=M
@2
M=D
@1
D=M
@i
D=D-M
M=M+1
@LOOP1
D;JGT
@END
0;JMP
(Z_BASE)
@2
M=0
@END
0;JMP
(Z_POW)
@2
M=1
@END
0;JMP
(ONE_BASE)
@2
M=1
@END
0;JMP
(ONE_POW)
@0
D=M
@2
M=D
@END
0;JMP
(END)
0;JMP
