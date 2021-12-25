import { BehaviorSubject } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AlertService {
  _messages$ = new BehaviorSubject<{ title: string; message: string }[]>([]);

  alert(title: string, msg: string) {
    const messages = this._messages$.value;
    const newMessages = [...messages, { title, message: msg }];
    this._messages$.next(newMessages);
  }

  clear(title: string) {
    const messages = this._messages$.value;
    const newMessages = messages.filter(m => m.title != title);
    this._messages$.next(newMessages);
  }
}
