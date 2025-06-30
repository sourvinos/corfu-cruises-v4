import { Directive, ElementRef } from '@angular/core'
import { FormControl, NgControl } from '@angular/forms'

@Directive({ selector: '[formControl], [formControlName]' })

export class InputMaxLengthDirective {

    constructor(private control: NgControl, private el: ElementRef) { }

    ngOnInit(): void {

        const err = this.control.control.validator ? this.control.control.validator(new FormControl('.'.repeat(10000000))) : null

        if (err && err.maxlength && err.maxlength.requiredLength)
            this.el.nativeElement.setAttribute('maxlength', err.maxlength.requiredLength)

    }

}