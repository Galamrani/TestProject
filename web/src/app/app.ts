import { Component, inject, signal } from '@angular/core';
import { Api, type Todo } from './api';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
})
export class App {
  private readonly api = inject(Api);

  protected readonly helloOutput = signal('');
  protected readonly todos = signal<Todo[]>([]);
  protected readonly error = signal('');

  constructor() {
    this.run(() => this.refresh());
  }

  protected sayHello(name: string) {
    this.run(async () => {
      const { message, time } = await this.api.hello(name);
      this.helloOutput.set(`${message} (server time: ${new Date(time).toLocaleTimeString()})`);
    });
  }

  protected addTodo(input: HTMLInputElement) {
    this.run(async () => {
      await this.api.addTodo(input.value);
      input.value = '';
      await this.refresh();
    });
  }

  protected toggleTodo(todo: Todo) {
    this.run(async () => {
      await this.api.toggleTodo(todo.id);
      await this.refresh();
    });
  }

  protected deleteTodo(todo: Todo) {
    this.run(async () => {
      await this.api.deleteTodo(todo.id);
      await this.refresh();
    });
  }

  private async refresh() {
    this.todos.set(await this.api.listTodos());
  }

  private async run(action: () => Promise<void>) {
    this.error.set('');
    try {
      await action();
    } catch (err) {
      this.error.set(err instanceof Error ? err.message : String(err));
    }
  }
}
