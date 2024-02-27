import React, { useState } from 'react';
import { IBase, ITaskElementsCollection, ITaskElement, IPage, IJournal, IRelation, IChecklistItem, ITimeLimit } from '../../../model/TasksService';
import { TaskController } from '../../../controllers/TaskController';
import CloseButton from '../Buttons/CloseButton/CloseButton';
import UpdateButton from '../Buttons/UpdateButton/UpdateButton';
import DeleteButton from '../Buttons/DeleteButton/DeleteButton';

interface UpdateTaskPopupProps {
    task: ITaskElement;
    onClose: () => void;
    onUpdate: (updatedTask: ITaskElementsCollection) => void;
    onDelete: (taskId: string) => void; // Define onDelete prop
  }

// const UpdateTaskPopup: React.FC<UpdateTaskPopupProps> = ({ task, onClose, onUpdate, onDelete }) => {
const UpdateTaskPopup: React.FC<UpdateTaskPopupProps> = ({ task, onClose, onUpdate, onDelete }) => {
    //   const [name, setName] = useState(task.baseElements[0].rid);
  const [name, setName] = useState(task.baseElements.name);
  const [description, setDescription] = useState(task.baseElements.description);
  const [updatedTask, setUpdatedTask] = useState<ITaskElement>(task);
  const [journalPages, setJournalPages] = useState<IPage[]>(task.journals ? task.journals.pages : []);

  // Update task elements
  const [baseElements, setBaseElements] = useState<IBase>(task.baseElements);
  const [journals, setJournals] = useState<IJournal | null>(task.journals);
  const [relations, setRelations] = useState<IRelation | null>(task.relations);
  const [checklists, setChecklists] = useState<IChecklistItem | null>(task.checklists);
  const [timeLimits, setTimeLimits] = useState<ITimeLimit | null>(task.timeLimits);

   const handleBaseChange = (updatedBase: IBase) => {
    setBaseElements(updatedBase);
  };

  const handleJournalChange = (updatedJournal: IJournal) => {
    setJournals(updatedJournal);
  };

  const handleRelationChange = (updatedRelation: IRelation) => {
    setRelations(updatedRelation);
  };

  const handleChecklistChange = (updatedChecklist: IChecklistItem) => {
    setChecklists(updatedChecklist);
  };

  const handleTimeLimitChange = (updatedTimeLimit: ITimeLimit) => {
    setTimeLimits(updatedTimeLimit);
  };


  const handleDelete = () => {
    // Call onDelete with the task ID
    onDelete(task.baseElements.bid);
  };

//   const handleUpdate = () => {
//     // const updatedTaskCollection: ITaskElementsCollection = {
//     //   ...taskCollection,
//     //   baseElements: [{ ...taskCollection.baseElements[0], name }],
//     //   journals: [{ ...taskCollection.journals[0], pages: [{ ...taskCollection.journals[0].pages[0], name: journalPageName }] }],
//     //   relations: [{ ...taskCollection.relations[0], relationType: parseInt(relationType) }],
//     //   checklists: [{ ...taskCollection.checklists[0], checked: checklistChecked === 'true' }],
//     //   timeLimits: [{ ...taskCollection.timeLimits[0], deadline: timeLimitDeadline }],
//     // };
//     // onUpdate(updatedTaskCollection);
//   };

    const handleUpdate = () => {
        const updatedTaskCollection: ITaskElementsCollection = {
        baseElements: [baseElements],
        journals: journals ? [journals] : [],
        relations: relations ? [relations] : [],
        checklists: checklists ? [checklists] : [],
        timeLimits: timeLimits ? [timeLimits] : []
        };
        console.log('updatedTaskCollection', updatedTaskCollection)
        onUpdate(updatedTaskCollection);
    };

  const handleAddPage = () => {
    setJournalPages([...journalPages, { name: '', content: '' }]);
  };

  const handleRemovePage = (index: number) => {
    const updatedPages = [...journalPages];
    updatedPages.splice(index, 1);
    setJournalPages(updatedPages);
  };

  const handlePageNameChange = (index: number, value: string) => {
    const updatedPages = [...journalPages];
    updatedPages[index].name = value;
    setJournalPages(updatedPages);
  };

  const handlePageContentChange = (index: number, value: string) => {
    const updatedPages = [...journalPages];
    updatedPages[index].content = value;
    setJournalPages(updatedPages);
  };

  return (
    <div className="popup">
    <div className="popup-inner">
    <h2>Update Task</h2>
    <div className="form-group">
        <label htmlFor="name">Base Name:</label>
        <input
            id="name"
            type="text"
            value={name}
            onChange={(e) => {
            setName(e.target.value);
            handleBaseChange({ ...baseElements, name: e.target.value });
            }}
        />
        </div>

        <div className="form-group">
        <label htmlFor="description">Base Description:</label>
        <input
            id="description"
            type="text"
            value={description}
            onChange={(e) => {
            setDescription(e.target.value);
            handleBaseChange({ ...baseElements, description: e.target.value });
            }}
        />
    </div>

        {/* Relations */}
        <div className="form-group">
        <label htmlFor="relationType">Relation Type:</label>
        <input
            id="relationType"
            type="number"
            value={relations ? relations.relationType : ''}
            onChange={(e) =>
            handleRelationChange(relations ? 
                { ...relations, relationType: parseInt(e.target.value) } : 
                { 
                relationType: parseInt(e.target.value), 
                relative: '',
                id: '',
                rid: task.baseElements.rid, 
                bid: task.baseElements.bid 
                }
            )
            }
        />
        </div>
        <div className="form-group">
        <label htmlFor="relative">Relative:</label>
        <input
            id="relative"
            type="text"
            value={relations ? relations.relative : ''}
            onChange={(e) => 
            handleRelationChange(relations ? 
                { ...relations, relative: e.target.value } : 
                { 
                relative: e.target.value, 
                relationType: 0, 
                id: '',
                rid: task.baseElements.rid, 
                bid: task.baseElements.bid 
                }
            )
            }
        />
        </div>

        {/* Render journal pages */}
        <label>Journal Pages:</label>
        {journalPages.map((page, index) => (
            <div key={index}>
            <label htmlFor={`pageName${index}`}>Name:</label>
            <input
                id={`pageName${index}`}
                type="text"
                value={page.name}
                onChange={(e) => handlePageNameChange(index, e.target.value)}
            />
            <label htmlFor={`pageContent${index}`}>Content:</label>
            <input
                id={`pageContent${index}`}
                type="text"
                value={page.content}
                onChange={(e) => handlePageContentChange(index, e.target.value)}
            />
            <button onClick={() => handleRemovePage(index)}>Remove Page</button>
            </div>
        ))}
        <button onClick={handleAddPage}>Add Page</button>

      {/* Render the time limit fields */}
        <div className="form-group">
        <label htmlFor="deadline">Deadline:</label>
        <input
            id="deadline"
            type="datetime-local"
            value={timeLimits ? timeLimits.deadline : ''}
            onChange={(e) =>
            handleTimeLimitChange(timeLimits ? 
                { ...timeLimits, deadline: e.target.value } : 
                { 
                    deadline: e.target.value.toString(), 
                    executionTime: '',
                    warningTime: '',
                    repeatSpan: '',
                    autoRepeat: false,  // Assuming a default value or use appropriate logic to determine
                    repeatCount: 0,     // Assuming a default value
                    active: false,      // Assuming a default value
                    repeated: false,    // Assuming a default value
                    id: '',
                    rid: task.baseElements.rid, 
                    bid: task.baseElements.bid 
                }
            )
            }
        />
        </div>

        <div className="form-group">
        <label htmlFor="executionTime">Execution Time:</label>
        <input
            id="executionTime"
            type="text"
            value={timeLimits ? timeLimits.executionTime : ''}
            onChange={(e) =>
            handleTimeLimitChange(timeLimits ? 
                { ...timeLimits, executionTime: e.target.value } : 
                { 
                    deadline: '',
                    executionTime: e.target.value,
                    warningTime: '',
                    repeatSpan: '',
                    autoRepeat: false,  // Assuming a default value or use appropriate logic to determine
                    repeatCount: 0,     // Assuming a default value
                    active: false,      // Assuming a default value
                    repeated: false,    // Assuming a default value
                    id: '',
                    rid: task.baseElements.rid, 
                    bid: task.baseElements.bid 
                }
            )
            }
        />
        </div>

        <div className="form-group">
        <label htmlFor="warningTime">Warning Time:</label>
        <input
            id="warningTime"
            type="text"
            value={timeLimits ? timeLimits.warningTime : ''}
            onChange={(e) =>
            handleTimeLimitChange(timeLimits ? 
                { ...timeLimits, warningTime: e.target.value } : 
                { 
                    deadline: '',
                    executionTime: '',
                    warningTime: e.target.value,
                    repeatSpan: '',
                    autoRepeat: false,  // Assuming a default value or use appropriate logic to determine
                    repeatCount: 0,     // Assuming a default value
                    active: false,      // Assuming a default value
                    repeated: false,    // Assuming a default value
                    id: '',
                    rid: task.baseElements.rid, 
                    bid: task.baseElements.bid 
                }
            )
            }
        />
        </div>

        <div className="form-group">
        <label htmlFor="repeatSpan">Repeat Span:</label>
        <input
            id="repeatSpan"
            type="text"
            value={timeLimits ? timeLimits.repeatSpan : ''}
            onChange={(e) =>
            handleTimeLimitChange(timeLimits ? 
                { ...timeLimits, repeatSpan: e.target.value } : 
                { 
                    deadline: '',
                    executionTime: '',
                    warningTime: '',
                    repeatSpan: e.target.value,
                    autoRepeat: false,  // Assuming a default value or use appropriate logic to determine
                    repeatCount: 0,     // Assuming a default value
                    active: false,      // Assuming a default value
                    repeated: false,    // Assuming a default value
                    id: '',
                    rid: task.baseElements.rid, 
                    bid: task.baseElements.bid 
                }
            )
            }
        />
    </div>

    <div className="form-group">
      <label htmlFor="checklistItem">Checklist parentID:</label>
      <input
        id="checklistItem"
        type="text"
        value={checklists ? checklists.parentID : ''}
        onChange={(e) =>
          handleChecklistChange(checklists ?
            { ...checklists, parentID: e.target.value } :
            {
                parentID: e.target.value,
                checked: false,
                id: '',
                rid: task.baseElements.rid, 
                bid: task.baseElements.bid, 
                resetOnComplete: true,              
            }
          )
        }
      />
    </div>
    <div className="form-group">
      <label htmlFor="checklistChecked">Checked:</label>
      <input
        id="checklistChecked"
        type="checkbox"
        checked={checklists ? checklists.checked : false}
        onChange={(e) =>
          handleChecklistChange(checklists ?
            { ...checklists, checked: e.target.checked } :
            {
                checked: e.target.checked,
                parentID: '',
                id: '',
                rid: task.baseElements.rid, 
                bid: task.baseElements.bid, 
                resetOnComplete: true,
            }
          )
        }
      />
    </div>

      <UpdateButton onClick={handleUpdate} />
      <CloseButton onClose={onClose} />
      <DeleteButton onDelete={handleDelete} />
    </div>
  </div>
  );

};

export default UpdateTaskPopup;