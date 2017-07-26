$(document).ready(function(){
	(function () {
		function TaskManager() {
			var formSelector = "#progressBoard";
			var intervalID;
			var taskTypes = {};
			var tasks = [];
			var timerInterval = 1000;
			var startWorkButtonSelector = "#startWork";
			var stopWorkButtonSelector = "#stopWork";
			var inputTaskNameSelector = "#inputName";
			var progressContainerSelector = "#showrogress";
			var addTaskButtonSelector = "#addtask";
			
			var template = '<div class="progress">  <div class="progress-bar" role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100" style="width: 60%;"> <span>60% Complete</span> </div></div>';
			
			var renderTaskProgress = function(task){	
				var result = $(template);
				result.find(".progress-bar").attr("aria-valuenow", task.GetProgress()).css("width", task.GetProgress()+'%').children().first().text(task.Name());
				return result;
			}
			
			var toggleStartStopButtons = function (isRunning){
				if(isRunning){
					$(startWorkButtonSelector).hide().prop("disabled",true);
					$(stopWorkButtonSelector).show().prop("disabled",false);
				}
				else{
					$(startWorkButtonSelector).show().prop("disabled",false);
					$(stopWorkButtonSelector).hide().prop("disabled",true);
				}
			}
			
			toggleStartStopButtons(false);
			
			 var setTaskTypes = function (types){
				taskTypes = types;
				for(var taskType in taskTypes){
					$(inputTaskNameSelector).append($('<option>', {
						value: taskType,
						text: taskTypes[taskType].name
					}));
				}
			}
			
			setTaskTypes(Tasks);
			
			$(addTaskButtonSelector).click((function(){
			var taskName = $(inputTaskNameSelector).val();	
				var task = new taskTypes[taskName](tasks.length);
				this.AddTask(task);
			}).bind(this));
			
			$(progressContainerSelector).click((function(){
				this.RenderProgress();
			}).bind(this));
			
			$(startWorkButtonSelector).click((function(){
				this.StartWork();
				toggleStartStopButtons(true);
			}).bind(this));
			
			$(stopWorkButtonSelector).click((function(){
				this.StopWork();
				toggleStartStopButtons(false);
			}).bind(this));
			
			this.StartWork = function(){
				if(!intervalID){
					intervalID = setInterval(function(){ 
						for(var index in tasks){
							if (tasks.hasOwnProperty(index)){
								tasks[index].DoStep();
							}
						}
					}, timerInterval);
				}
			};
			
			this.StopWork = function(){
				clearInterval(intervalID);
				intervalID = '';
			}
			
			this.AddTask = function (task) {
				tasks.push(task);
			};
				
			this.RenderProgress = function(){
				$(formSelector).empty();
				for(var index in tasks){
					if (tasks.hasOwnProperty(index)){
						var taskprogress = renderTaskProgress(tasks[index]);
						$(formSelector).append(taskprogress);
					}
				}
			}		
		}
		this.TaskManager = new TaskManager();
	})();
});