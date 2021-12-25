import { CameraService } from './../camera.service';
import Hls from 'hls.js';
import { ElementRef, OnDestroy } from '@angular/core';
import { Component, Input, ViewChild, OnInit, ViewEncapsulation } from '@angular/core';
// import JSMpeg from '@cycjimmy/jsmpeg-player';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'sh-camera-card',
  templateUrl: './camera-card.component.html',
  styleUrls: ['./camera-card.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CameraCardComponent implements OnInit, OnDestroy {
  // @ViewChild('stream', { static: true }) stream;
  @Input() link:string;
  @Input() name : string;
  @ViewChild('video', { static: true }) private readonly video: ElementRef<HTMLVideoElement>;

  constructor(public translate : TranslateService, private cameraService: CameraService){}

  ngOnInit() {
    this.create()
  }

  create() {
    const video = this.video.nativeElement;

    if (video.canPlayType('application/vnd.apple.mpegurl')) {
      video.src = this.link;
      video.play();
    } else {
      const hls = new Hls({
        progressive: false,
      });

      hls.on(Hls.Events.ERROR, (evt, data) => {
        if (data.fatal) {
          hls.destroy();

          setTimeout(() => {
            this.create();
          }, 2000);
        }
      });

      hls.loadSource(this.link);
      hls.attachMedia(video);

      video.play();
    }
  }

  ngOnDestroy() {
    // destroy player

  }
}
