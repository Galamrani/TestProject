import { Injectable } from '@angular/core';

export interface Todo {
  id: number;
  title: string;
  done: boolean;
}

async function request<T>(path: string, init?: RequestInit): Promise<T> {
  const res = await fetch(`/api${path}`, {
    headers: { 'Content-Type': 'application/json' },
    ...init,
  });
  if (!res.ok) {
    const body = await res.json().catch(() => null);
    throw new Error(body?.error ?? `${res.status} ${res.statusText}`);
  }
  return res.status === 204 ? (undefined as T) : res.json();
}

@Injectable({ providedIn: 'root' })
export class Api {
  hello = (name: string) =>
    request<{ message: string; time: string }>(`/hello?name=${encodeURIComponent(name)}`);
  listTodos = () => request<Todo[]>('/todos');
  addTodo = (title: string) =>
    request<Todo>('/todos', { method: 'POST', body: JSON.stringify({ title }) });
  toggleTodo = (id: number) => request<Todo>(`/todos/${id}/toggle`, { method: 'PUT' });
  deleteTodo = (id: number) => request<void>(`/todos/${id}`, { method: 'DELETE' });
}
