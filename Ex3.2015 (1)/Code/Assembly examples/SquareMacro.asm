

// Draws a square at the top-left corner of the screen.
// The square is R0 chars wide and R0 chars high.

   D=M[0]
   D;JLE:INFINITE_LOOP 
   M[j]=D
   @SCREEN
   D=A
   M[row_address]=D

(OUTER_LOOP)
   D=M[row_address]
   M[address]=D
   D=M[0]
   M[i]=D	

(INNER_LOOP)
   @88
   D=A
   A=M[address]
   M=D
   M[address]=M[address]+1
   @i
   MD=M-1
   D;JGT:INNER_LOOP

   @80
   D=A
   A=M[row_address]
   D=D+A
   M[row_address]=D

   @j
   MD=M-1
   D;JGT:OUTER_LOOP

(INFINITE_LOOP)
   0;JMP:INFINITE_LOOP
