import { CameraService } from './camera.service';
import { finalize } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { of, Subscription } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'sh-cameras',
  templateUrl: './camera.component.html',
  styleUrls: ['./camera.component.scss'],
})
export class CameraComponent implements OnInit, OnDestroy {
  cameras = [];
  isLoading: boolean;
  hasError: boolean;
  selectedLink: string;
  lastSelectedLink: string;
  private _subscription: Subscription[] = [];
  constructor(private http: HttpClient, private cameraService: CameraService) {}

  ngOnInit() {
    // this.getCameras();
    this.cameras = [
      {
        name: 'Big Buck Bunny',
        link: 'https://multiplatform-f.akamaihd.net/i/multi/will/bunny/big_buck_bunny_,640x360_400,640x360_700,640x360_1000,950x540_1500,.f4v.csmil/master.m3u8',
      },
      {
        name: 'Tears of Steel',
        link: 'https://demo.unified-streaming.com/video/tears-of-steel/tears-of-steel.ism/.m3u8',
      },
      {
        name: 'Sintel',
        link: 'https://multiplatform-f.akamaihd.net/i/multi/april11/sintel/sintel-hd_,512x288_450_b,640x360_700_b,768x432_1000_b,1024x576_1400_m,.mp4.csmil/master.m3u8',
      },
      {
        name: 'Eight',
        link: 'https://moctobpltc-i.akamaihd.net/hls/live/571329/eight/playlist.m3u8',
      },
      {
        name: 'apple',
        link: 'https://devstreaming-cdn.apple.com/videos/streaming/examples/img_bipbop_adv_example_fmp4/master.m3u8',
      },
      {
        name: 'Akamaized',
        link: 'https://cph-p2p-msl.akamaized.net/hls/live/2000341/test/master.m3u8',
      },
    ];
    this.selectedLink = this.cameras[0].link;

    this.getCameras()

    this.cameraService.setup.subscribe(set => {
      if (!this.lastSelectedLink && this.cameras[0]) this.lastSelectedLink = this.cameras[0].link

      if (set) this.selectedLink = this.lastSelectedLink
      else {
        this.lastSelectedLink = this.selectedLink;
        this.selectedLink = null;
      }
    });
  }

  getCameras() {
    if (this.isLoading) return;

    this.isLoading = true;
    this.hasError = false;
    const request = this.http
      .get(`${environment.apiUrl}/Camera`)
      .pipe(
        tap((res: any) => {
          res.forEach(re => this.cameras.push({
            'name' : re.cameraName ,
            'link' : re.ip+'/stream.m3u8'
          }))
          this.hasError = false;
        }),
        catchError((err: any) => {
          this.hasError = true;
          return of(null);
        }),
        finalize(() => {
          this.isLoading = false;
        })
      )
      .subscribe();

    this._subscription.push(request);
  }

  getIcon() {
    return {
      fa: true,
      'fa-search': !this.isLoading,
      'fa-circle-notch': this.isLoading,
      'fa-spin': this.isLoading,
    };
  }

  ngOnDestroy() {
    this._subscription.forEach((sb) => sb.unsubscribe());
  }
}
