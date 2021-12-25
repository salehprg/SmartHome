import { NgModule, Component, Input, forwardRef, EventEmitter, Output, ChangeDetectorRef, ChangeDetectionStrategy, ViewEncapsulation, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { RGBService } from 'src/app/modules/home/components/rgb/_services/rgb.service';

export const INPUTSWITCH_VALUE_ACCESSOR: any = {
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => SwitchComponent),
    multi: true
};

@Component({
    selector: 'sh-switch',
    template: `
        <div [ngClass]="{'switch': true, 'switch-checked': checked}" (click)="onClick($event, cb)">
  <div class="hidden-accessible">
    <input #cb type="checkbox" [checked]="checked" (change)="onInputChange($event)"
      role="switch" [attr.aria-checked]="checked" />
  </div>
  <span  #slider class="switch-slider"></span>
</div>

    `,
    providers: [INPUTSWITCH_VALUE_ACCESSOR],
    changeDetection: ChangeDetectionStrategy.OnPush,
    encapsulation: ViewEncapsulation.None
})
export class SwitchComponent implements ControlValueAccessor, OnInit {

    @ViewChild('slider' , {static : true}) slider ;

    ngOnInit() {
        if (this.isRgb) {
            this.rgbService._color$.subscribe(color => {
                this.color = color
                if(this.checked) {
                    this.slider.nativeElement.style.background = this.color
                }else{
                    this.slider.nativeElement.style.background = 'var(--surface-0)'
                }
            })

            this.rgbService._switch_checked$.subscribe(sw => {
                this.checked = sw

                if(this.checked) {
                    this.slider.nativeElement.style.background = this.color
                }else{
                    this.slider.nativeElement.style.background = 'var(--surface-0)'
                }
            })

        }
    }

    color: string;

    @Input() isRgb: boolean;

    @Input() style: any;

    @Input() styleClass: string;

    @Input() tabindex: number;

    @Input() inputId: string;

    @Input() name: string;

    @Input() disabled: boolean;

    @Input() readonly: boolean;

    @Input() ariaLabelledBy: string;

    @Output() onChange: EventEmitter<any> = new EventEmitter();

    checked: boolean = false;

    focused: boolean = false;

    onModelChange: Function = () => { };

    onModelTouched: Function = () => { };

    constructor(private cd: ChangeDetectorRef, private rgbService: RGBService) { }

    onClick(event: Event, cb: HTMLInputElement) {
        if (!this.disabled && !this.readonly) {
            event.preventDefault();
            this.toggle(event);
            cb.focus();
        }
    }

    onInputChange(event: Event) {
        if (!this.readonly) {
            const inputChecked = (<HTMLInputElement>event.target).checked;
            this.updateModel(event, inputChecked);
        }
    }

    toggle(event: Event) {
        this.updateModel(event, !this.checked);

        if(this.isRgb){
            //
            this.rgbService._switch_checked$.next(this.checked)
            if(!this.checked){
                this.slider.nativeElement.style.background = 'var(--surface-0)'

            }
            else{
                this.slider.nativeElement.style.background = this.color
            }
        }

    }

    updateModel(event: Event, value: boolean) {
        this.checked = value;
        this.onModelChange(this.checked);
        this.onChange.emit({
            originalEvent: event,
            checked: this.checked
        });
    }

    onFocus(event: Event) {
        this.focused = true;
    }

    onBlur(event: Event) {
        this.focused = false;
        this.onModelTouched();
    }

    writeValue(checked: any): void {
        this.checked = checked;
        this.cd.markForCheck();
    }

    registerOnChange(fn: Function): void {
        this.onModelChange = fn;
    }

    registerOnTouched(fn: Function): void {
        this.onModelTouched = fn;
    }

    setDisabledState(val: boolean): void {
        this.disabled = val;
        this.cd.markForCheck();
    }
}
