import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { NewTask, UserTask } from './userTask';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'TaskApp';

  public newTaskText: string;
  public tasks: UserTask[] = [];
  public displayError: boolean = false;

  constructor(private http: HttpClient) {    
  }

  async ngOnInit(): Promise<void> {
    await this.getAllTasks();
  }

  public async addTask(): Promise<void> {
    let newTask: NewTask = {
      text: this.newTaskText,
    };
    this.newTaskText = "";

    try {
      await this.http.post("https://localhost:5001/api/tasks", newTask).toPromise();
      await this.getAllTasks();
    }
    catch {
      this.displayError = true;
    }
  }

  private async getAllTasks(): Promise<void> {
    this.tasks = await this.http.get<UserTask[]>("https://localhost:5001/api/tasks").toPromise();
  }

  public async removeTask(taskId: string): Promise<void> {
    await this.http.delete(`https://localhost:5001/api/tasks/${taskId}`).toPromise();
    await this.getAllTasks();
  }
}
