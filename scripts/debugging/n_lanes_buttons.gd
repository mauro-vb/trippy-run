extends Control

@onready
var moreB = $"-"
@onready
var lessB = $"+"

@export 
var lanes_handler : Node2D

func _ready():
	moreB.pressed.connect(add_lane)
	lessB.pressed.connect(remove_lane)
	
func add_lane():
	lanes_handler.nLanes += 1

func remove_lane():
	lanes_handler.nLanes -= 1
