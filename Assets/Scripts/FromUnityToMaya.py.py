import maya.cmds as cmds
import json

# example filepath: C:\Users\Name\Desktop\ToMaya.json

# get the path name from the textField
def start(*args):
    pathname = cmds.textField(textF, query=True, text=True)
    if pathname != '':
        test(pathname)
    
# creates the input window 
cmds.window( width=300 )
cmds.columnLayout( adjustableColumn=True )
cmds.button( label='Start', command=start )
cmds.text(label='Path of the .json file:')
textF = cmds.textField()
cmds.showWindow()

def test(filepath):
	data = []
	i = 0
	with open(filepath) as fp:
		line = fp.readline()
		while line:
			line = line.rstrip()
			if line != 'COLOR' and line != 'BREAK':
				temp = line.split(',')
				data.append(temp)
				# creates the vertex array with the curve's vertex position
			line = fp.readline()
			line = line.rstrip()
			if line == 'COLOR':
			    # creates a curve with unity's line's position
				curva = cmds.curve(p=data)
				name = "curva" + str(i)
				cmds.rename(curva, name)
				cmds.scale(0.01, 0.01, 0.01)
				cmds.makeIdentity(apply=True, t=1,r=1,s=1,n=0)
				line = fp.readline()
				line = line.rstrip()
				temp = line.split(',')
				cmds.setAttr(name + ".overrideEnabled", True)
				cmds.setAttr(name + ".overrideRGBColors", True )
				t = []
				for item in temp:
				    t.append(float(item))
				for channel, color_val in zip(('R', 'G', 'B'),t):
				    cmds.setAttr(name+'.overrideColor{}'.format(channel),color_val)
                #t contains the color data
				line = fp.readline()
				line = line.rstrip()
				del data[:]
				i += 1
		fp.close()