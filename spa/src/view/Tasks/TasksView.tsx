// TasksView.js
import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import CreateTaskPopup from '../components/CreateTaskPopup/CreateTaskPopup';
import './TasksView.css';
import { TaskController } from '../../controllers/TaskController';
import { IBase, IJournal, IRelation, IChecklistItem, ITimeLimit, ITaskElementsCollection, ITaskElement } from '../../model/TasksService';
import UpdateTaskPopup from '../components/UpdateTaskPopup/UpdateTaskPopup';

const TasksView = () => {
  const { rid } = useParams();
  const navigate = useNavigate();
  const [tasks, setTasks] = useState<ITaskElementsCollection | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [isAuthorized, setIsAuthorized] = useState<boolean | null>(null);
  const [showCreateTaskPopup, setShowCreateTaskPopup] = useState(false);
  const [showUpdatePopup, setShowUpdatePopup] = useState(false);
  const [selectedTask, setSelectedTask] = useState<ITaskElement | null>(null);
  const [taskElements, setTaskElements] = useState<ITaskElement[]>([]);




  // Fetch tasks as collections
  const fetchTasks = async () => {
    setLoading(true);
    try {
      const tasksData = await TaskController.getTasks();
      if (rid) {
        const filteredTasks = {
          baseElements: tasksData.baseElements.filter((be) => be.rid === rid),
          journals: tasksData.journals.filter((j) => j.rid === rid),
          relations: tasksData.relations.filter((rel) => rel.rid === rid),
          checklists: tasksData.checklists.filter((cl) => cl.rid === rid),
          timeLimits: tasksData.timeLimits.filter((tl) => tl.rid === rid)
        };
        const filteredSingleTasksArray = transformTasksToTaskElements(filteredTasks);
        setTasks(filteredTasks);
        setTaskElements(filteredSingleTasksArray);
        console.log('filteredTasks', filteredTasks);
        console.log('filteredSingleTasksArray', filteredSingleTasksArray);
      }
    } catch (error) {
      console.error('Error fetching tasks:', error);
    } finally {
      setLoading(false);
    }
  };

  const checkAuthorization = () => {
    const authorizedRrid = localStorage.getItem('rid');
    const bearerToken = localStorage.getItem('bearerToken');

    if (!bearerToken || authorizedRrid !== rid) {
      setIsAuthorized(false);  // Not authorized
      alert("You are not authorized to view these tasks.");
      navigate('/roots');
    } else {
      setIsAuthorized(true);  // Authorized
    }
  };

  useEffect(() => {
    checkAuthorization();
    fetchTasks();
  }, [rid, navigate]);

  function transformTasksToTaskElements(tasks: ITaskElementsCollection): ITaskElement[] {
    return tasks.baseElements.map((baseElement): ITaskElement => {
      // Find the matching journals, relations, checklist items, and time limits based on bid
      const matchingJournals = tasks.journals.filter(journal => journal.bid === baseElement.bid);
      const matchingRelations = tasks.relations.filter(relation => relation.bid === baseElement.bid);
      const matchingChecklistItems = tasks.checklists.filter(checklist => checklist.bid === baseElement.bid);
      const matchingTimeLimits = tasks.timeLimits.filter(timeLimit => timeLimit.bid === baseElement.bid);
  
      // Construct a new ITaskElement with the matched items
      const taskElement: ITaskElement = {
        baseElements: baseElement,
        journals: matchingJournals.length > 0 ? matchingJournals[0] : null, // assuming only one journal per baseElement
        relations: matchingRelations.length > 0 ? matchingRelations[0] : null, // assuming only one relation per baseElement
        checklists: matchingChecklistItems.length > 0 ? matchingChecklistItems[0] : null, // assuming only one checklist item per baseElement
        timeLimits: matchingTimeLimits.length > 0 ? matchingTimeLimits[0] : null, // assuming only one time limit per baseElement
      };
  
      return taskElement;
    });
  }

  const handleCreateTask = () => {
    setShowCreateTaskPopup(true);
  };
  
  const handleTaskCreated = () => {
    setShowCreateTaskPopup(false);
    // Refresh tasks list logic here...
    fetchTasks();
  };

  const handleTaskClick = (task: ITaskElement) => {
    setSelectedTask(task);
    setShowUpdatePopup(true);
  };

  // This function is triggered when the update popup is meant to be closed
  const handleCloseUpdatePopup = () => {
    setShowUpdatePopup(false); // Close the popup
    setSelectedTask(null); // Clear the selected task
  };

  const handleDelete = async (taskId: string) => {
    if (!taskId) return;
    await TaskController.deleteTask(taskId);
    alert("Task deleted.");
    // Refetch tasks to update the list
    fetchTasks();
    // Close the update popup
    setShowUpdatePopup(false);
  };

  // const handleTaskClick = (tasks: ITaskElementsCollection) => {
  //   if (isAuthorized && tasks.baseElements[0].rid === localStorage.getItem('rid')) {
  //     navigate(`/roots/${task.rid}`); // instead open popup to update
  //   } else {
  //     setSelectedTask(tasks);
  //   }
  // };

  const handleUpdate = async (taskCollection: ITaskElementsCollection) => {
    const authorizedRrid = localStorage.getItem('rid');
    const bearerToken = localStorage.getItem('bearerToken');
    if (!bearerToken || authorizedRrid !== rid) return;
    await TaskController.updateTasks(taskCollection);
    alert("Task updated.");
    fetchTasks();  // Refresh the root details
    setShowUpdatePopup(false);  // Close the popup
  };

  

  return (
    <div>
      <h2>Tasks for Root {rid}</h2>
      <button onClick={() => setShowCreateTaskPopup(true)}>Create Task</button>
      {loading ? <p>Loading...</p> : (
        taskElements && 
        <div className="task-grid-container">
        <div className="task-grid">
          {taskElements.map((taskElement) => (
             <div className="task-item" key={taskElement.baseElements.bid} onClick={() => handleTaskClick(taskElement)}>
              {taskElement.baseElements &&
              <div>
                <div className='task-header task-element'>
                [{taskElement.baseElements.rid} | {taskElement.baseElements.bid}] {taskElement.baseElements.name}
                </div>
                <div className='task-description task-element'>
                  {taskElement.baseElements.description}
                </div>
              </div>
              }
              {/* Render the relation if it exists */}
              <div className='relations-wrapper'>
                {taskElement.relations && 
                  <div className='relations task-element'>
                    {taskElement.relations.relationType == 0 ? 'P' : 'C'} [{taskElement.relations.id} | {taskElement.relations.relationType}] {taskElement.baseElements.name}
                  </div>
                }
              </div>
              {/* Render the journal if it exists */}
              {taskElement.journals && taskElement.journals.pages && 
                <div className='journals-wrapper task-element'>
                  {taskElement.journals.pages.map((page, index) => (
                    <div className='journal-name'>
                      [{taskElement.journals?.id} | {index+1}] {taskElement.journals?.pages[index].name}
                      <div>
                        {taskElement.journals?.pages[index].content}
                      </div>
                    </div>
                  ))}
                </div>
              }
              {/* Render the time limit if it exists */}
              {taskElement.timeLimits && 
                <div className='timelimits task-element'>
                  [{taskElement.timeLimits.id}]
                  d: {taskElement.timeLimits.deadline}
                  e: {taskElement.timeLimits.executionTime}
                  w: {taskElement.timeLimits.warningTime}
                  r: {taskElement.timeLimits.repeatSpan}
                  rc: {taskElement.timeLimits.repeatCount}
                </div>
              }
              {/* Render the checklist if it exists */}
              {taskElement.checklists && 
                <div className='checklist task-element'>
                  <input
                    id="checklistChecked"
                    type="checkbox"
                    checked={taskElement.checklists.checked == true ? true : false}
                  />
                  completed
                </div>
              }
            </div>
          ))}
      </div>
      </div>
      )}
      {showUpdatePopup && selectedTask && (
      <UpdateTaskPopup
        task={selectedTask}
        onClose={handleCloseUpdatePopup}
        onUpdate={handleUpdate}
        onDelete={handleDelete}
      />
    )}
      {showCreateTaskPopup && rid && (
        <CreateTaskPopup
          rid={rid}
          onClose={() => setShowCreateTaskPopup(false)}
          onTaskCreated={handleTaskCreated}
        />
      )}
      {/* Display tasks here */}
    </div>
  );
};

export default TasksView;