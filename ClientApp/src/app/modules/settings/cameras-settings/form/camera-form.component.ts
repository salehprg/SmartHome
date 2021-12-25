import { HttpClient } from "@angular/common/http";
import { Component, OnInit, ViewChild, ViewEncapsulation } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { TranslateService } from "@ngx-translate/core";
import { NotifierService } from "angular-notifier";
import Keyboard from 'simple-keyboard';
import layout from 'simple-keyboard-layouts/build/layouts/farsi'
import { environment } from "src/environments/environment";
@Component({
    selector: 'sh-camera-setting-form',
    templateUrl: './camera-form.component.html',
    encapsulation : ViewEncapsulation.None ,
    styleUrls: ['./camera-form.component.scss',
        "../../../../../../node_modules/simple-keyboard/build/css/index.css",]
})

export class CameraSettingForm implements OnInit{
    @ViewChild('notif' , {static : true}) notifTemp ;

    keyboard : Keyboard;
    inputName = 'name' ;
    inputs = {
        name : '',
        ip : ''
    }

    lang : string = 'persian'
    ipEditable : boolean = false;

    constructor(private active : ActivatedRoute ,
        public translate : TranslateService ,
        private router : Router ,
         private http: HttpClient,
         private notifier : NotifierService){}
    ngOnInit(){
        if(this.active.snapshot.params.id){
            //get camera info from server
            //edit camera
            this.http.get(`${environment.apiUrl}/Camera`).subscribe(
                (res:any)=>{

                    const camera = res.find(cam => cam.id === +this.active.snapshot.params.id)


                    this.inputs.name = camera.cameraName
                    this.inputs.ip = camera.ip.toString()
                }
            )
            this.ipEditable = false
        }
        else{
            this.ipEditable = true
        }
    }

    ngAfterViewInit(){
        this.keyboard = new Keyboard({
            inputName : this.inputName ,
            onChange : (input) => this.onChange(input),
            onKeyPress : (button) => this.onKeyPress(button) ,
            ...layout ,
            theme: 'hg-theme-default myTheme1',
            preventMouseDownDefault : true
        });
        this.keyboard.replaceInput(this.inputs)
        document.getElementById('camera-keyboard').style.width = '100%'
    }

    onInputFocus = (event:any) => {
        this.inputName = event.target.id;

        if(this.inputName === 'name'){
            if(this.lang === 'persian'){
                this.keyboard.setOptions({
                    inputName : event.target.id,
                    ...layout,
                })

            }
            else {
               this.keyboard.setOptions({
                   inputName : event.target.id,
                   layout: {
                    default: [
                        "` 1 2 3 4 5 6 7 8 9 0 - = {bksp}",
                        "{tab} q w e r t y u i o p [ ] \\",
                        "{lock} a s d f g h j k l ; ' {enter}",
                        "{shift} z x c v b n m , . / {shift}",
                        ".com @ {space}",
                    ],
                    shift: [
                        "~ ! @ # $ % ^ & * ( ) _ + {bksp}",
                        "{tab} Q W E R T Y U I O P { } |",
                        '{lock} A S D F G H J K L : " {enter}',
                        "{shift} Z X C V B N M < > ? {shift}",
                        ".com @ {space}",
                    ],
                },
               })
            }
            document.getElementById('camera-keyboard').style.width = '100%'
        }
        else{
            this.keyboard.setOptions({
                inputName : event.target.id ,
                layout: {
                    default: ["1 2 3", "4 5 6", "7 8 9", "0 . {bksp}"],
                    // shift: ["! / #", "$ % ^", "& * (", "{shift} ) +", "{bksp}"]
                },
            })
            document.getElementById('camera-keyboard').style.width = '30%'
            document.getElementById('camera-keyboard').style.marginLeft = 'auto'
            document.getElementById('camera-keyboard').style.marginRight = 'auto'
        }
    }

    onInputChange = (event : any) => {
        this.keyboard.setInput(event.target.value , event.target.id)
    }

    onChange = (input:string) =>{
        this.inputs[this.inputName] = input;
    }

    onKeyPress = (button:string) =>{
        if(button === "{shift}" || button === "{lock}") this.handleShift();
    }

    changeLang(event){
        this.lang = event.target.value;

        if(this.lang === 'english' && this.inputName !== 'ip'){
            this.keyboard.setOptions({
                layout: {
                    default: [
                        "` 1 2 3 4 5 6 7 8 9 0 - = {bksp}",
                        "{tab} q w e r t y u i o p [ ] \\",
                        "{lock} a s d f g h j k l ; ' {enter}",
                        "{shift} z x c v b n m , . / {shift}",
                        ".com @ {space}",
                    ],
                    shift: [
                        "~ ! @ # $ % ^ & * ( ) _ + {bksp}",
                        "{tab} Q W E R T Y U I O P { } |",
                        '{lock} A S D F G H J K L : " {enter}',
                        "{shift} Z X C V B N M < > ? {shift}",
                        ".com @ {space}",
                    ],
                },
            })
        }
        else if(this.lang === 'persian' && this.inputName !== 'ip'){
            this.keyboard.setOptions({
                ...layout,
            })
        }
    }

    handleShift(){
        let currentLayout = this.keyboard.options.layoutName
        let shiftToggle = currentLayout === 'default' ? 'shift' : 'default'

        this.keyboard.setOptions({
            layoutName : shiftToggle
        })
    }

    goToDetails() {
        this.router.navigateByUrl('/details')
    }

    handleEvent(){
        if(this.ipEditable){
            //add camera
            if(this.inputs.name.trim().length > 0 && this.inputs.ip.trim().length > 0){
                this.http.put(`${environment.apiUrl}/Camera` , {
                    cameraName : this.inputs.name ,
                    ip : this.inputs.ip
                }).subscribe(
                    res =>{
                        this.notifier.notify('success' , this.translate.instant('NOTIFICATION.ADDED_SUCCESSFULLY'))
                        setTimeout(()=>{
                            this.router.navigateByUrl('/settings')
                        } , 2000)

                    },
                    err =>{
                        this.notifier.notify('error' , this.translate.instant('NOTIFICATION.MODULE_CHANGE_ERROR'))

                    }
                )
            }
        }
        else {
            //edit camera

            if(this.inputs.name.trim().length > 0 ){
                this.http.post(`${environment.apiUrl}/Camera` , {
                    id : +this.active.snapshot.params.id ,
                    cameraName : this.inputs.name ,
                    ip : this.inputs.ip
                }).subscribe(
                    res=>{
                        this.notifier.notify('success' , this.translate.instant('NOTIFICATION.CHANGED_SUCCESSFULLY'))

                        setTimeout(() => {
                            this.router.navigateByUrl('/settings')
                        } , 2000)

                    },
                    err =>{
                        this.notifier.notify('error' , this.translate.instant('NOTIFICATION.MODULE_ADD_ERROR'))

                    }
                )
            }
        }
    }
 }

