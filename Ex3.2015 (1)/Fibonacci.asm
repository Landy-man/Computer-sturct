//writes the 20 first numbers of the fibonacci series in cells 100-119

@120
D=A
@stop
M=D
@100
M=1
@temp
M=1
@101
D=A
M=1
@temp2
M=1
@place
M=D+1
(LOOP)
@temp
D=M
@temp2
D=D+M
@place
A=M
M=D
@temp3
M=D
@temp2
D=M
@temp
M=D
@temp3
D=M
@temp2
M=D
@place
M=M+1
@stop
D=M
@place
D=D-M
@LOOP
D;JGT
(END)
@END
0;JMP
