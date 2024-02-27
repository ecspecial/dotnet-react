import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import '../../App.css';
import './Grid.css';

const Grid = () => {
  // Extend the ITask interface to include journals
  interface ITask {
    rid: string;
    bid: string;
    name: string;
    description: string;
    journals?: IJournal[]; // Make journals an optional property
  }

  // Define an interface for a Page if needed
  interface IPage {
    content: string; // Replace with actual structure
  }
  
  // Define an interface for the journal
  interface IJournal {
    id: string;
    rid: string;
    bid: string;
    pages: IPage[]; // Now using the IPage interface
  }

  // Define the types for the update request structure
  type UpdateTask = {
    baseElements: {
      bid: string;
      rid: string;
      name: string;
      description: string;
    }[];
    journals: {
      id: string;
      rid: string;
      bid: string;
      pages: any[]; // Define the type for pages if required
    }[];
    relations: {
      id: string;
      rid: string;
      bid: string;
      relative: string;
      relationType: number;
    }[];
    checklists: {
      id: string;
      rid: string;
      bid: string;
      checked: boolean;
      resetOnComplete: boolean;
      parentID: string;
    }[];
    timeLimits: {
      id: string;
      rid: string;
      bid: string;
      deadline: string;
      executionTime: {
        ticks: number;
        days: number;
        hours: number;
        milliseconds: number;
        minutes: number;
        seconds: number;
      };
      warningTime: {
        ticks: number;
        days: number;
        hours: number;
        milliseconds: number;
        minutes: number;
        seconds: number;
      };
      autoRepeat: boolean;
      repeatCount: number;
      active: boolean;
      repeated: boolean;
    }[];
  };

  const [showCreateForm, setShowCreateForm] = useState(false);
  const [showUpdateForm, setShowUpdateForm] = useState(false);
  const [updateTaskData, setUpdateTaskData] = useState<ITask | null>(null);
  const [taskName, setTaskName] = useState('');
  const [taskDescription, setTaskDescription] = useState('');
  const [journalId, setJournalId] = useState('');
  const [relationRelative, setRelationRelative] = useState('');
  const [checklistChecked, setChecklistChecked] = useState(false);
  const [timeLimitDeadline, setTimeLimitDeadline] = useState('');
  const [tasks, setTasks] = useState<ITask[]>([]);
  // Add new state for journals
  const [journalPages, setJournalPages] = useState<any[]>([]); // Define a more specific type if needed
  const navigate = useNavigate();

  const handleCreateTask = async () => {
    const bearerToken = localStorage.getItem('bearerToken');
    const rid = localStorage.getItem('rid');
  
    if (!bearerToken || !rid) {
      alert('Please authorize to create tasks');
      navigate('/');
      return;
    }
  
    try {
      const response = await axios.post(`http://localhost:7777/api/v1/roots/${rid}/tasks`, {
        name: taskName,
        description: taskDescription,
      }, {
        headers: { Authorization: `Bearer ${bearerToken}` },
      });
  
      const newTask = {
        rid: rid,
        bid: response.data.bid,
        name: taskName,
        description: taskDescription,
        journals: [] // Initialize an empty array for journals
      };
  
      setTasks([...tasks, newTask]);
      setTaskName('');
      setTaskDescription('');
      setShowCreateForm(false);
      // Set the newly created task for updating
      setUpdateTaskData(newTask);
      setShowUpdateForm(true); // Show the update form with journal fields
      alert('Task created successfully');
    } catch (error) {
      console.error('Error creating task:', error);
    }
  };

  // Function to handle input change for journals
  const handleJournalChange = (index: number, field: keyof IJournal, value: string) => {
    setUpdateTaskData((prevTaskData) => {
      if (!prevTaskData) return prevTaskData;

      const updatedJournals = prevTaskData.journals?.map((journal, jIndex) => {
        if (index === jIndex) {
          return { ...journal, [field]: value };
        }
        return journal;
      });

      return { ...prevTaskData, journals: updatedJournals };
    });
  };

  // Function to handle input change for journal pages
  const handlePageChange = (journalIndex: number, pageIndex: number, content: string) => {
    setUpdateTaskData((prevTaskData) => {
      if (!prevTaskData) return prevTaskData;

      const updatedJournals = prevTaskData.journals?.map((journal, jIndex) => {
        if (journalIndex === jIndex) {
          const updatedPages = journal.pages.map((page, pIndex) => {
            if (pageIndex === pIndex) {
              return { content };
            }
            return page;
          });
          return { ...journal, pages: updatedPages };
        }
        return journal;
      });

      return { ...prevTaskData, journals: updatedJournals };
    });
  };

  const handleUpdateTask = async () => {
    if (!updateTaskData) return;
    
    // Prepare the JSON structure according to the API expectations
    const updateData = {
      baseElements: [
        {
          bid: updateTaskData.bid,
          rid: updateTaskData.rid,
          name: taskName,
          description: taskDescription,
        },
      ],
      journals: updateTaskData.journals || [],
      relations: [], // Empty array
      checklists: [], // Empty array
      timeLimits: [], // Empty array
    };
    
    try {
      const response = await axios.patch(
        `http://localhost:7777/api/v1/roots/${updateTaskData.rid}/tasks`,
        updateData,
        {
          headers: { Authorization: `Bearer ${localStorage.getItem('bearerToken')}` },
        }
      );
    
      // Update tasks state with the new task data
      setTasks(tasks.map(task => task.bid === updateTaskData.bid ? { ...task, name: taskName, description: taskDescription } : task));
      // Reset states and close the form
      setUpdateTaskData(null);
      setShowUpdateForm(false);
      alert('Task updated successfully');
    } catch (error) {
      console.error('Error updating task:', error);
    }
  };

  const handleShowUpdateForm = (task: ITask) => {
    setUpdateTaskData({
      ...task,
      journals: task.journals ? [...task.journals] : [] // Ensure journals is an array
    });
    setTaskName(task.name);
    setTaskDescription(task.description);
    setShowUpdateForm(true);
  };

  const handleCancelUpdate = () => {
    setTaskName('');
    setTaskDescription('');
    setShowUpdateForm(false);
  };

  const handleCancelCreate = () => {
    setTaskName('');
    setTaskDescription('');
    setShowCreateForm(false);
  };

  const handleGetAllTasks = async () => {
    const rid = localStorage.getItem('rid');
    const bearerToken = localStorage.getItem('bearerToken');
  
    if (!bearerToken || !rid) {
      alert('Please authorize to get tasks');
      navigate('/');
      return;
    }
  
    try {
      const response = await axios.get(`http://localhost:7777/api/v1/roots/${rid}/tasks`, {
        headers: { Authorization: `Bearer ${bearerToken}` },
      });
  
      // Assuming the response data is the array of tasks
      // Assuming the response data includes journals within the task structure
      setTasks(response.data.baseElements.map((task: any) => ({
        rid: task.rid,
        bid: task.bid,
        name: task.name,
        description: task.description,
        journals: task.journals, // Assign journals data here
      })));
    } catch (error) {
      console.error('Error getting tasks:', error);
    }
  };
  

  return (
    <div className='appContainer'>
      <h2 className='title'>Grid</h2>
      <div className='buttonContainer'>
        {showCreateForm ? (
          <>
            <button className='button' onClick={handleCreateTask}>Submit</button>
            <button className='button cancel-task-button' onClick={handleCancelCreate}>Cancel</button>
          </>
        ) : (
          <>
            <button className='button create-task-button' onClick={() => setShowCreateForm(true)}>Create New Task</button>
            <button className='button get-tasks-button' onClick={handleGetAllTasks}>Get all tasks</button>
          </>
        )}
      </div>
      <div className='taskGrid'>
        {tasks.map((task, index) => (
          <div className='taskCard' key={index}>
            <p>RID: {task.rid}</p>
            <p>BID: {task.bid}</p>
            <p>Name: {task.name}</p>
            <p>Description: {task.description}</p>
            <button className='button update-task-button' onClick={() => handleShowUpdateForm(task)}>Update Task</button>
          </div>
        ))}
      </div>
      {showCreateForm && (
        <div className='form'>
          <input
            className='input'
            value={taskName}
            onChange={(e) => setTaskName(e.target.value)}
            placeholder="Task Name"
          />
          <input
            className='input'
            value={taskDescription}
            onChange={(e) => setTaskDescription(e.target.value)}
            placeholder="Task Description"
          />
          {updateTaskData?.journals?.map((journal, index) => (
            <div key={index}>
              <label htmlFor={`update-journal-${index}`}>Journal ID {index+1}:</label>
              <input
                id={`update-journal-${index}`}
                className='input'
                value={journal.id}
                onChange={(e) => {
                  // Create a new array with the updated journal
                  const newJournals = [...(updateTaskData.journals || [])];
                  newJournals[index] = { ...journal, id: e.target.value };
                  setUpdateTaskData({ ...updateTaskData, journals: newJournals });
                }}
                placeholder="Journal ID"
              />
              {/* Render input fields for pages or other journal attributes here */}
            </div>
          ))}
        </div>
      )}
      {showUpdateForm && (
        <div className='form'>
          <label htmlFor="update-task-name">Task Name:</label>
          <input
            id="update-task-name"
            className='input'
            value={taskName}
            onChange={(e) => setTaskName(e.target.value)}
            placeholder="Task Name"
          />
          <label htmlFor="update-task-description">Task Description:</label>
          <input
            id="update-task-description"
            className='input'
            value={taskDescription}
            onChange={(e) => setTaskDescription(e.target.value)}
            placeholder="Task Description"
          />
          {/* Add additional input fields here for other task properties */}
          {updateTaskData?.journals?.map((journal, jIndex) => (
            <div key={jIndex}>
              <label htmlFor={`update-journal-id-${jIndex}`}>Journal ID:</label>
              <input
                id={`update-journal-id-${jIndex}`}
                className='input'
                value={journal.id}
                onChange={(e) => handleJournalChange(jIndex, 'id', e.target.value)}
                placeholder="Journal ID"
              />
              {journal.pages.map((page, pIndex) => (
                <div key={pIndex}>
                  <label htmlFor={`update-journal-page-${jIndex}-${pIndex}`}>Page {pIndex + 1} Content:</label>
                  <input
                    id={`update-journal-page-${jIndex}-${pIndex}`}
                    className='input'
                    value={page.content}
                    onChange={(e) => handlePageChange(jIndex, pIndex, e.target.value)}
                    placeholder={`Page ${pIndex + 1} Content`}
                  />
                </div>
              ))}
              {/* Add a button to add new pages or other journal properties if needed */}
            </div>
          ))}
          {/* Relations */}
          <label htmlFor="update-task-relation-relative">Relation Relative:</label>
          <input
            id="update-task-relation-relative"
            className='input'
            value={relationRelative}
            onChange={(e) => setRelationRelative(e.target.value)}
            placeholder="Relation Relative"
          />
          {/* Checklists */}
          <label htmlFor="update-task-checklist-checked">Checklist Checked:</label>
          <input
            id="update-task-checklist-checked"
            className='input'
            type="checkbox"
            checked={checklistChecked}
            onChange={(e) => setChecklistChecked(e.target.checked)}
          />
          {/* Time Limits */}
          <label htmlFor="update-task-timeLimit-deadline">Time Limit Deadline:</label>
          <input
            id="update-task-timeLimit-deadline"
            className='input'
            type="datetime-local"
            value={timeLimitDeadline}
            onChange={(e) => setTimeLimitDeadline(e.target.value)}
          />
          {/* Add other fields as needed */}
          <button className='button' onClick={handleUpdateTask}>Submit</button>
          <button className='button cancel-task-button' onClick={handleCancelUpdate}>Cancel</button>
        </div>
      )}
    </div>
  );
};

export default Grid;