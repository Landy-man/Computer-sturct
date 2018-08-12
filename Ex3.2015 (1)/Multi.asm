// R2=R0*R1

@0
D=M
@Zero
D; JEQ

@1
D=M
@Zero
D; JEQ

@i
M=1
@2
M=0


(LOOP)
@0
D=M
@2
M=D+M
@1
D=M
@i
D=D-M
M=M+1
@LOOP
D; JGT
@END
0;JMP


(ZERO)
@2
M=0


(END)
@END
0;JMP