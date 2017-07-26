function Task(number) {	
	this.index = number;
	this.progress = 0;
}

Task.prototype.GetProgress = function() {
	return this.progress;
}

Task.prototype.DoStep = function(){
	this.progress += 0.2 * this.speed;
	if(this.progress > 100)
	{
		this.progress = 100;
	}
};

Task.prototype.Name = function(){
	return "#" + this.index + " - " + this.title;
};

function FastTask() {
	Task.apply(this, arguments)
	this.title = "FastTask";
	this.speed = 25;
}

function SlowTask() {
	Task.apply(this, arguments)
	this.title = "SlowTask";
	this.speed = 7;
}

function MediumTask() {
	Task.apply(this, arguments)
	this.title = "MediumTask";
	this.speed = 15;
}

function ChaoticTask() {
	Task.apply(this, arguments)
	this.title = "ChaoticTask";
	this.speed = Math.random() * 30;
}

FastTask.prototype = Object.create(Task.prototype);
SlowTask.prototype = Object.create(Task.prototype);
MediumTask.prototype = Object.create(Task.prototype);
ChaoticTask.prototype = Object.create(Task.prototype);

window.Tasks = [SlowTask, MediumTask, FastTask, ChaoticTask];