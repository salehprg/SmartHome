import { BehaviorSubject } from 'rxjs';

import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class CameraService {
  setup = new BehaviorSubject<boolean>(null);

  play() {
    this.setup.next(true);
  }

  pause() {
    this.setup.next(false);
  }
  // currentVideo: any = null;

  // play() {
  //   try {
  //     this.currentVideo.play();
  //   } catch(err) {}
  // }

  // pause() {
  //   try {
  //     this.currentVideo.pause();
  //   } catch(err) {}
  // }
}
