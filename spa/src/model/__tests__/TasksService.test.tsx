import axios from 'axios';
import { AppConfig } from '../config';
import { IBase, IBaseDTO, IChecklistItem, IJournal, IRelation, ITaskElementsCollection, TasksService } from '../TasksService';

jest.mock('axios');
const mockedAxios = axios as jest.Mocked<typeof axios>;

describe('Tasks service', () => {
    let config: AppConfig = require('./../config.json');

    afterEach(() => {
        jest.clearAllMocks();
        });

    test('test get tasks', async () => {
        // Arrange
        let rid = 'test_rid';
        localStorage.setItem('rid', rid);
        let taskBase: IBase = { rid: rid, bid: 'test_bid', name: 'test_name', description: 'test_description' };
        let taskRelation: IRelation = { id: 'test_relation_id', rid: rid, bid: taskBase.bid, relative: 'test_relative', relationType: 0  };
        let taskJournal: IJournal = { id: 'test_journal_id', rid: rid, bid: taskBase.bid, pages: [] };
        let taskChecklistItem: IChecklistItem = { id: 'test_checklist_id', rid: rid, bid: taskBase.bid, checked: false, resetOnComplete: true, parentID: 'test_id' };

        let getTaskCollection :ITaskElementsCollection = { baseElements: [taskBase], journals: [taskJournal], relations: [taskRelation], checklists: [taskChecklistItem], timeLimits: [] };

        mockedAxios.get.mockResolvedValueOnce({
          data: { baseElements: [taskBase], journals: [taskJournal], relations: [taskRelation], checklists: [taskChecklistItem], timeLimits: [] }
        });

        // Act
        let result = await TasksService.getTasks();

        // Assert
        expect(mockedAxios.get).toHaveBeenCalledWith(config.apiURI + `/roots/${rid}/tasks`, {"headers": {"Authorization": "Bearer null"}});
        expect(result).toEqual(getTaskCollection);
        });

    test('test create task base', async () => {
        // Arrange
        let rid = 'test_rid';
        let testBase: IBaseDTO = {rid: rid, name: "name", description: "description"};
        localStorage.setItem('rid', rid);
        mockedAxios.post.mockResolvedValueOnce({ data: { rid: rid, bid:'test_bid', name: "name", description: "description" } });
        // Act
        await TasksService.createTaskBase(testBase);

        // Assert
        expect(mockedAxios.post).toHaveBeenCalledWith(config.apiURI + `/roots/${testBase.rid}/tasks`, { name: testBase.name, description: testBase.description  }, {"headers": {"Authorization": "Bearer null"}});
        });

    test('test update task', async () => {
        // Arrange
        let rid = 'test_rid';
        localStorage.setItem('rid', rid);
        let taskBase: IBase = { rid: rid, bid: 'test_bid', name: 'test_name', description: 'test_description' };
        let taskRelation: IRelation = { id: 'test_relation_id', rid: rid, bid: taskBase.bid, relative: 'test_relative', relationType: 0  };
        let taskJournal: IJournal = { id: 'test_journal_id', rid: rid, bid: taskBase.bid, pages: [] };
        let taskChecklistItem: IChecklistItem = { id: 'test_checklist_id', rid: rid, bid: taskBase.bid, checked: false, resetOnComplete: true, parentID: 'test_id' };

        let updateTaskCollection :ITaskElementsCollection = { baseElements: [taskBase], journals: [taskJournal], relations: [taskRelation], checklists: [taskChecklistItem], timeLimits: [] };

        // Act
        await TasksService.updateTasks(updateTaskCollection);

        // Assert
        expect(mockedAxios.patch).toHaveBeenCalledWith(config.apiURI + `/roots/${rid}/tasks`, { baseElements: [taskBase], journals: [taskJournal], relations: [taskRelation], checklists: [taskChecklistItem], timeLimits: [] }, {"headers": {"Authorization": "Bearer null"}});
        });

    test('test delete root', async () => {
        // Arrange
        let rid = 'test_rid';
        let bid = 'test_bid';
        localStorage.setItem('rid', rid); 

        // Act
        await TasksService.deleteTask(bid);

        // Assert
        expect(mockedAxios.delete).toHaveBeenCalledWith(config.apiURI + `/roots/${rid}/tasks/${bid}`, {"headers": {"Authorization": "Bearer null"}});
        });
});
