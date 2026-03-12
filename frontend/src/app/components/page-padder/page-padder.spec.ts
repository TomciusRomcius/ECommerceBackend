import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PagePadder } from './page-padder';

describe('PagePadder', () => {
  let component: PagePadder;
  let fixture: ComponentFixture<PagePadder>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PagePadder]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PagePadder);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
