﻿namespace ZeldaEditor.ObjectsEditor {
	partial class ObjectEditor {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.groupBoxCustomObjectControls = new System.Windows.Forms.GroupBox();
			this.panelCustomObjectControl = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.textBoxId = new System.Windows.Forms.TextBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.checkBoxPoofEffect = new System.Windows.Forms.CheckBox();
			this.checkBoxStartDisabled = new System.Windows.Forms.CheckBox();
			this.numberBoxWidth = new System.Windows.Forms.NumericUpDown();
			this.comboBoxSolidType = new System.Windows.Forms.ComboBox();
			this.numberBoxHeight = new System.Windows.Forms.NumericUpDown();
			this.comboBoxMovementType = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.comboBoxLedgeDirection = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.comboBoxCollisionModel = new System.Windows.Forms.ComboBox();
			this.label7 = new System.Windows.Forms.Label();
			this.labelLedgeDirection = new System.Windows.Forms.Label();
			this.tabPageTileInteractions = new System.Windows.Forms.TabPage();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.labelMoveDirection = new System.Windows.Forms.Label();
			this.checkBoxDisableOnDestroy = new System.Windows.Forms.CheckBox();
			this.labelBraceletLevel = new System.Windows.Forms.Label();
			this.checkBoxBurnable = new System.Windows.Forms.CheckBox();
			this.checkBoxBombable = new System.Windows.Forms.CheckBox();
			this.labelSwordLevel = new System.Windows.Forms.Label();
			this.checkBoxDigable = new System.Windows.Forms.CheckBox();
			this.comboBoxMoveDirection = new System.Windows.Forms.ComboBox();
			this.comboBoxBraceletLevel = new System.Windows.Forms.ComboBox();
			this.comboBoxSwordLevel = new System.Windows.Forms.ComboBox();
			this.checkBoxMoveOnce = new System.Windows.Forms.CheckBox();
			this.checkBoxBreakOnSwitch = new System.Windows.Forms.CheckBox();
			this.checkBoxSwitchable = new System.Windows.Forms.CheckBox();
			this.checkBoxBoomerangable = new System.Windows.Forms.CheckBox();
			this.checkBoxPickupable = new System.Windows.Forms.CheckBox();
			this.checkBoxMovable = new System.Windows.Forms.CheckBox();
			this.checkBoxCuttable = new System.Windows.Forms.CheckBox();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.eventsTab = new ZeldaEditor.ObjectsEditor.ObjectEditorEventsTab();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.rawPropertyGrid = new ZeldaEditor.PropertiesEditor.ZeldaPropertyGrid();
			this.panel1 = new System.Windows.Forms.Panel();
			this.buttonApply = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonDone = new System.Windows.Forms.Button();
			this.comboBoxResetCondition = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.groupBoxCustomObjectControls.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numberBoxWidth)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numberBoxHeight)).BeginInit();
			this.tabPageTileInteractions.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPageTileInteractions);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(567, 394);
			this.tabControl1.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.splitContainer1);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(559, 368);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "General";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.IsSplitterFixed = true;
			this.splitContainer1.Location = new System.Drawing.Point(3, 3);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.groupBoxCustomObjectControls);
			this.splitContainer1.Panel1.Controls.Add(this.label1);
			this.splitContainer1.Panel1.Controls.Add(this.textBoxId);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
			this.splitContainer1.Size = new System.Drawing.Size(553, 362);
			this.splitContainer1.SplitterDistance = 270;
			this.splitContainer1.TabIndex = 16;
			// 
			// groupBoxCustomObjectControls
			// 
			this.groupBoxCustomObjectControls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBoxCustomObjectControls.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.groupBoxCustomObjectControls.Controls.Add(this.panelCustomObjectControl);
			this.groupBoxCustomObjectControls.Location = new System.Drawing.Point(3, 50);
			this.groupBoxCustomObjectControls.Name = "groupBoxCustomObjectControls";
			this.groupBoxCustomObjectControls.Padding = new System.Windows.Forms.Padding(10);
			this.groupBoxCustomObjectControls.Size = new System.Drawing.Size(264, 309);
			this.groupBoxCustomObjectControls.TabIndex = 15;
			this.groupBoxCustomObjectControls.TabStop = false;
			this.groupBoxCustomObjectControls.Text = "groupBox4";
			// 
			// panelCustomObjectControl
			// 
			this.panelCustomObjectControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panelCustomObjectControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelCustomObjectControl.Location = new System.Drawing.Point(10, 23);
			this.panelCustomObjectControl.MinimumSize = new System.Drawing.Size(100, 0);
			this.panelCustomObjectControl.Name = "panelCustomObjectControl";
			this.panelCustomObjectControl.Size = new System.Drawing.Size(244, 276);
			this.panelCustomObjectControl.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(10, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(21, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "ID:";
			// 
			// textBoxId
			// 
			this.textBoxId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxId.Location = new System.Drawing.Point(59, 11);
			this.textBoxId.Name = "textBoxId";
			this.textBoxId.Size = new System.Drawing.Size(208, 20);
			this.textBoxId.TabIndex = 0;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.checkBoxPoofEffect);
			this.groupBox2.Controls.Add(this.checkBoxStartDisabled);
			this.groupBox2.Controls.Add(this.numberBoxWidth);
			this.groupBox2.Controls.Add(this.comboBoxSolidType);
			this.groupBox2.Controls.Add(this.numberBoxHeight);
			this.groupBox2.Controls.Add(this.comboBoxResetCondition);
			this.groupBox2.Controls.Add(this.comboBoxMovementType);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.comboBoxLedgeDirection);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.comboBoxCollisionModel);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.labelLedgeDirection);
			this.groupBox2.Location = new System.Drawing.Point(3, 3);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(273, 356);
			this.groupBox2.TabIndex = 11;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "General";
			// 
			// checkBoxPoofEffect
			// 
			this.checkBoxPoofEffect.AutoSize = true;
			this.checkBoxPoofEffect.Location = new System.Drawing.Point(116, 19);
			this.checkBoxPoofEffect.Name = "checkBoxPoofEffect";
			this.checkBoxPoofEffect.Size = new System.Drawing.Size(135, 17);
			this.checkBoxPoofEffect.TabIndex = 0;
			this.checkBoxPoofEffect.Text = "Spawn with poof effect";
			this.checkBoxPoofEffect.UseVisualStyleBackColor = true;
			// 
			// checkBoxStartDisabled
			// 
			this.checkBoxStartDisabled.AutoSize = true;
			this.checkBoxStartDisabled.Location = new System.Drawing.Point(11, 19);
			this.checkBoxStartDisabled.Name = "checkBoxStartDisabled";
			this.checkBoxStartDisabled.Size = new System.Drawing.Size(95, 17);
			this.checkBoxStartDisabled.TabIndex = 4;
			this.checkBoxStartDisabled.Text = "Starts disabled";
			this.checkBoxStartDisabled.UseVisualStyleBackColor = true;
			// 
			// numberBoxWidth
			// 
			this.numberBoxWidth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.numberBoxWidth.Location = new System.Drawing.Point(116, 47);
			this.numberBoxWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numberBoxWidth.Name = "numberBoxWidth";
			this.numberBoxWidth.Size = new System.Drawing.Size(151, 20);
			this.numberBoxWidth.TabIndex = 12;
			this.numberBoxWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// comboBoxSolidType
			// 
			this.comboBoxSolidType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboBoxSolidType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxSolidType.FormattingEnabled = true;
			this.comboBoxSolidType.Location = new System.Drawing.Point(116, 99);
			this.comboBoxSolidType.Name = "comboBoxSolidType";
			this.comboBoxSolidType.Size = new System.Drawing.Size(151, 21);
			this.comboBoxSolidType.TabIndex = 10;
			this.comboBoxSolidType.SelectedIndexChanged += new System.EventHandler(this.comboBoxSolidType_SelectedIndexChanged);
			// 
			// numberBoxHeight
			// 
			this.numberBoxHeight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.numberBoxHeight.Location = new System.Drawing.Point(116, 73);
			this.numberBoxHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numberBoxHeight.Name = "numberBoxHeight";
			this.numberBoxHeight.Size = new System.Drawing.Size(151, 20);
			this.numberBoxHeight.TabIndex = 12;
			this.numberBoxHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// comboBoxMovementType
			// 
			this.comboBoxMovementType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboBoxMovementType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxMovementType.FormattingEnabled = true;
			this.comboBoxMovementType.Location = new System.Drawing.Point(116, 180);
			this.comboBoxMovementType.Name = "comboBoxMovementType";
			this.comboBoxMovementType.Size = new System.Drawing.Size(151, 21);
			this.comboBoxMovementType.TabIndex = 10;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 75);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(38, 13);
			this.label3.TabIndex = 1;
			this.label3.Text = "Height";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(8, 49);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(38, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Width:";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(6, 102);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(60, 13);
			this.label5.TabIndex = 1;
			this.label5.Text = "Solid Type:";
			// 
			// comboBoxLedgeDirection
			// 
			this.comboBoxLedgeDirection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboBoxLedgeDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxLedgeDirection.FormattingEnabled = true;
			this.comboBoxLedgeDirection.Location = new System.Drawing.Point(116, 126);
			this.comboBoxLedgeDirection.Name = "comboBoxLedgeDirection";
			this.comboBoxLedgeDirection.Size = new System.Drawing.Size(151, 21);
			this.comboBoxLedgeDirection.TabIndex = 10;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(8, 183);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(96, 13);
			this.label6.TabIndex = 1;
			this.label6.Text = "Environment Type:";
			// 
			// comboBoxCollisionModel
			// 
			this.comboBoxCollisionModel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboBoxCollisionModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxCollisionModel.FormattingEnabled = true;
			this.comboBoxCollisionModel.Location = new System.Drawing.Point(116, 153);
			this.comboBoxCollisionModel.Name = "comboBoxCollisionModel";
			this.comboBoxCollisionModel.Size = new System.Drawing.Size(151, 21);
			this.comboBoxCollisionModel.TabIndex = 10;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(8, 156);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(80, 13);
			this.label7.TabIndex = 1;
			this.label7.Text = "Collision Model:";
			// 
			// labelLedgeDirection
			// 
			this.labelLedgeDirection.AutoSize = true;
			this.labelLedgeDirection.Location = new System.Drawing.Point(6, 129);
			this.labelLedgeDirection.Name = "labelLedgeDirection";
			this.labelLedgeDirection.Size = new System.Drawing.Size(82, 13);
			this.labelLedgeDirection.TabIndex = 1;
			this.labelLedgeDirection.Text = "Ledge Direction";
			// 
			// tabPageTileInteractions
			// 
			this.tabPageTileInteractions.Controls.Add(this.groupBox3);
			this.tabPageTileInteractions.Location = new System.Drawing.Point(4, 22);
			this.tabPageTileInteractions.Name = "tabPageTileInteractions";
			this.tabPageTileInteractions.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageTileInteractions.Size = new System.Drawing.Size(559, 368);
			this.tabPageTileInteractions.TabIndex = 4;
			this.tabPageTileInteractions.Text = "Interactions";
			this.tabPageTileInteractions.UseVisualStyleBackColor = true;
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.labelMoveDirection);
			this.groupBox3.Controls.Add(this.checkBoxDisableOnDestroy);
			this.groupBox3.Controls.Add(this.labelBraceletLevel);
			this.groupBox3.Controls.Add(this.checkBoxBurnable);
			this.groupBox3.Controls.Add(this.checkBoxBombable);
			this.groupBox3.Controls.Add(this.labelSwordLevel);
			this.groupBox3.Controls.Add(this.checkBoxDigable);
			this.groupBox3.Controls.Add(this.comboBoxMoveDirection);
			this.groupBox3.Controls.Add(this.comboBoxBraceletLevel);
			this.groupBox3.Controls.Add(this.comboBoxSwordLevel);
			this.groupBox3.Controls.Add(this.checkBoxMoveOnce);
			this.groupBox3.Controls.Add(this.checkBoxBreakOnSwitch);
			this.groupBox3.Controls.Add(this.checkBoxSwitchable);
			this.groupBox3.Controls.Add(this.checkBoxBoomerangable);
			this.groupBox3.Controls.Add(this.checkBoxPickupable);
			this.groupBox3.Controls.Add(this.checkBoxMovable);
			this.groupBox3.Controls.Add(this.checkBoxCuttable);
			this.groupBox3.Location = new System.Drawing.Point(256, 6);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(297, 356);
			this.groupBox3.TabIndex = 12;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Player Interactions";
			// 
			// labelMoveDirection
			// 
			this.labelMoveDirection.AutoSize = true;
			this.labelMoveDirection.Location = new System.Drawing.Point(159, 167);
			this.labelMoveDirection.Name = "labelMoveDirection";
			this.labelMoveDirection.Size = new System.Drawing.Size(79, 13);
			this.labelMoveDirection.TabIndex = 10;
			this.labelMoveDirection.Text = "Move Direction";
			// 
			// checkBoxDisableOnDestroy
			// 
			this.checkBoxDisableOnDestroy.AutoSize = true;
			this.checkBoxDisableOnDestroy.Location = new System.Drawing.Point(6, 329);
			this.checkBoxDisableOnDestroy.Name = "checkBoxDisableOnDestroy";
			this.checkBoxDisableOnDestroy.Size = new System.Drawing.Size(113, 17);
			this.checkBoxDisableOnDestroy.TabIndex = 0;
			this.checkBoxDisableOnDestroy.Text = "Disable on destroy";
			this.checkBoxDisableOnDestroy.UseVisualStyleBackColor = true;
			// 
			// labelBraceletLevel
			// 
			this.labelBraceletLevel.AutoSize = true;
			this.labelBraceletLevel.Location = new System.Drawing.Point(159, 94);
			this.labelBraceletLevel.Name = "labelBraceletLevel";
			this.labelBraceletLevel.Size = new System.Drawing.Size(119, 13);
			this.labelBraceletLevel.TabIndex = 10;
			this.labelBraceletLevel.Text = "Minimum Bracelet Level";
			// 
			// checkBoxBurnable
			// 
			this.checkBoxBurnable.AutoSize = true;
			this.checkBoxBurnable.Location = new System.Drawing.Point(6, 191);
			this.checkBoxBurnable.Name = "checkBoxBurnable";
			this.checkBoxBurnable.Size = new System.Drawing.Size(68, 17);
			this.checkBoxBurnable.TabIndex = 0;
			this.checkBoxBurnable.Text = "Burnable";
			this.checkBoxBurnable.UseVisualStyleBackColor = true;
			// 
			// checkBoxBombable
			// 
			this.checkBoxBombable.AutoSize = true;
			this.checkBoxBombable.Location = new System.Drawing.Point(6, 214);
			this.checkBoxBombable.Name = "checkBoxBombable";
			this.checkBoxBombable.Size = new System.Drawing.Size(73, 17);
			this.checkBoxBombable.TabIndex = 0;
			this.checkBoxBombable.Text = "Bombable";
			this.checkBoxBombable.UseVisualStyleBackColor = true;
			// 
			// labelSwordLevel
			// 
			this.labelSwordLevel.AutoSize = true;
			this.labelSwordLevel.Location = new System.Drawing.Point(159, 44);
			this.labelSwordLevel.Name = "labelSwordLevel";
			this.labelSwordLevel.Size = new System.Drawing.Size(110, 13);
			this.labelSwordLevel.TabIndex = 10;
			this.labelSwordLevel.Text = "Minimum Sword Level";
			// 
			// checkBoxDigable
			// 
			this.checkBoxDigable.AutoSize = true;
			this.checkBoxDigable.Location = new System.Drawing.Point(6, 237);
			this.checkBoxDigable.Name = "checkBoxDigable";
			this.checkBoxDigable.Size = new System.Drawing.Size(62, 17);
			this.checkBoxDigable.TabIndex = 0;
			this.checkBoxDigable.Text = "Digable";
			this.checkBoxDigable.UseVisualStyleBackColor = true;
			// 
			// comboBoxMoveDirection
			// 
			this.comboBoxMoveDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxMoveDirection.FormattingEnabled = true;
			this.comboBoxMoveDirection.Location = new System.Drawing.Point(28, 164);
			this.comboBoxMoveDirection.Name = "comboBoxMoveDirection";
			this.comboBoxMoveDirection.Size = new System.Drawing.Size(125, 21);
			this.comboBoxMoveDirection.TabIndex = 9;
			// 
			// comboBoxBraceletLevel
			// 
			this.comboBoxBraceletLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxBraceletLevel.FormattingEnabled = true;
			this.comboBoxBraceletLevel.Location = new System.Drawing.Point(28, 91);
			this.comboBoxBraceletLevel.Name = "comboBoxBraceletLevel";
			this.comboBoxBraceletLevel.Size = new System.Drawing.Size(125, 21);
			this.comboBoxBraceletLevel.TabIndex = 9;
			// 
			// comboBoxSwordLevel
			// 
			this.comboBoxSwordLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxSwordLevel.FormattingEnabled = true;
			this.comboBoxSwordLevel.Location = new System.Drawing.Point(28, 41);
			this.comboBoxSwordLevel.Name = "comboBoxSwordLevel";
			this.comboBoxSwordLevel.Size = new System.Drawing.Size(125, 21);
			this.comboBoxSwordLevel.TabIndex = 9;
			// 
			// checkBoxMoveOnce
			// 
			this.checkBoxMoveOnce.AutoSize = true;
			this.checkBoxMoveOnce.Location = new System.Drawing.Point(28, 141);
			this.checkBoxMoveOnce.Name = "checkBoxMoveOnce";
			this.checkBoxMoveOnce.Size = new System.Drawing.Size(106, 17);
			this.checkBoxMoveOnce.TabIndex = 0;
			this.checkBoxMoveOnce.Text = "Only Move Once";
			this.checkBoxMoveOnce.UseVisualStyleBackColor = true;
			// 
			// checkBoxBreakOnSwitch
			// 
			this.checkBoxBreakOnSwitch.AutoSize = true;
			this.checkBoxBreakOnSwitch.Location = new System.Drawing.Point(28, 306);
			this.checkBoxBreakOnSwitch.Name = "checkBoxBreakOnSwitch";
			this.checkBoxBreakOnSwitch.Size = new System.Drawing.Size(104, 17);
			this.checkBoxBreakOnSwitch.TabIndex = 0;
			this.checkBoxBreakOnSwitch.Text = "Break on Switch";
			this.checkBoxBreakOnSwitch.UseVisualStyleBackColor = true;
			// 
			// checkBoxSwitchable
			// 
			this.checkBoxSwitchable.AutoSize = true;
			this.checkBoxSwitchable.Checked = true;
			this.checkBoxSwitchable.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxSwitchable.Location = new System.Drawing.Point(6, 283);
			this.checkBoxSwitchable.Name = "checkBoxSwitchable";
			this.checkBoxSwitchable.Size = new System.Drawing.Size(107, 17);
			this.checkBoxSwitchable.TabIndex = 0;
			this.checkBoxSwitchable.Text = "Switch Hookable";
			this.checkBoxSwitchable.UseVisualStyleBackColor = true;
			this.checkBoxSwitchable.CheckedChanged += new System.EventHandler(this.checkBoxSwitchable_CheckedChanged);
			// 
			// checkBoxBoomerangable
			// 
			this.checkBoxBoomerangable.AutoSize = true;
			this.checkBoxBoomerangable.Location = new System.Drawing.Point(6, 260);
			this.checkBoxBoomerangable.Name = "checkBoxBoomerangable";
			this.checkBoxBoomerangable.Size = new System.Drawing.Size(132, 17);
			this.checkBoxBoomerangable.TabIndex = 0;
			this.checkBoxBoomerangable.Text = "Magic Boomerangable";
			this.checkBoxBoomerangable.UseVisualStyleBackColor = true;
			// 
			// checkBoxPickupable
			// 
			this.checkBoxPickupable.AutoSize = true;
			this.checkBoxPickupable.Checked = true;
			this.checkBoxPickupable.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxPickupable.Location = new System.Drawing.Point(6, 68);
			this.checkBoxPickupable.Name = "checkBoxPickupable";
			this.checkBoxPickupable.Size = new System.Drawing.Size(79, 17);
			this.checkBoxPickupable.TabIndex = 0;
			this.checkBoxPickupable.Text = "Pickupable";
			this.checkBoxPickupable.UseVisualStyleBackColor = true;
			this.checkBoxPickupable.CheckedChanged += new System.EventHandler(this.checkBoxPickupable_CheckedChanged);
			// 
			// checkBoxMovable
			// 
			this.checkBoxMovable.AutoSize = true;
			this.checkBoxMovable.Checked = true;
			this.checkBoxMovable.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxMovable.Location = new System.Drawing.Point(6, 118);
			this.checkBoxMovable.Name = "checkBoxMovable";
			this.checkBoxMovable.Size = new System.Drawing.Size(67, 17);
			this.checkBoxMovable.TabIndex = 0;
			this.checkBoxMovable.Text = "Movable";
			this.checkBoxMovable.UseVisualStyleBackColor = true;
			this.checkBoxMovable.CheckedChanged += new System.EventHandler(this.checkBoxMovable_CheckedChanged);
			// 
			// checkBoxCuttable
			// 
			this.checkBoxCuttable.AutoSize = true;
			this.checkBoxCuttable.Checked = true;
			this.checkBoxCuttable.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxCuttable.Location = new System.Drawing.Point(6, 19);
			this.checkBoxCuttable.Name = "checkBoxCuttable";
			this.checkBoxCuttable.Size = new System.Drawing.Size(112, 17);
			this.checkBoxCuttable.TabIndex = 0;
			this.checkBoxCuttable.Text = "Cuttable by Sword";
			this.checkBoxCuttable.UseVisualStyleBackColor = true;
			this.checkBoxCuttable.CheckedChanged += new System.EventHandler(this.checkBoxCuttable_CheckedChanged);
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.eventsTab);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(559, 368);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Events";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// eventsTab
			// 
			this.eventsTab.Dock = System.Windows.Forms.DockStyle.Fill;
			this.eventsTab.Location = new System.Drawing.Point(3, 3);
			this.eventsTab.Name = "eventsTab";
			this.eventsTab.Size = new System.Drawing.Size(553, 362);
			this.eventsTab.TabIndex = 0;
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.rawPropertyGrid);
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage3.Size = new System.Drawing.Size(559, 368);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Raw Properties";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// rawPropertyGrid
			// 
			this.rawPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rawPropertyGrid.EditorControl = null;
			this.rawPropertyGrid.Location = new System.Drawing.Point(3, 3);
			this.rawPropertyGrid.Name = "rawPropertyGrid";
			this.rawPropertyGrid.Size = new System.Drawing.Size(553, 362);
			this.rawPropertyGrid.TabIndex = 4;
			this.rawPropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.rawPropertyGrid_PropertyValueChanged);
			// 
			// panel1
			// 
			this.panel1.AutoSize = true;
			this.panel1.Controls.Add(this.buttonApply);
			this.panel1.Controls.Add(this.buttonCancel);
			this.panel1.Controls.Add(this.buttonDone);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 394);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(567, 29);
			this.panel1.TabIndex = 1;
			// 
			// buttonApply
			// 
			this.buttonApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonApply.Location = new System.Drawing.Point(407, 3);
			this.buttonApply.Name = "buttonApply";
			this.buttonApply.Size = new System.Drawing.Size(75, 23);
			this.buttonApply.TabIndex = 2;
			this.buttonApply.Text = "Apply";
			this.buttonApply.UseVisualStyleBackColor = true;
			this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(488, 3);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 1;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// buttonDone
			// 
			this.buttonDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonDone.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonDone.Location = new System.Drawing.Point(326, 3);
			this.buttonDone.Name = "buttonDone";
			this.buttonDone.Size = new System.Drawing.Size(75, 23);
			this.buttonDone.TabIndex = 0;
			this.buttonDone.Text = "Done";
			this.buttonDone.UseVisualStyleBackColor = true;
			this.buttonDone.Click += new System.EventHandler(this.buttonDone_Click);
			// 
			// comboBoxResetCondition
			// 
			this.comboBoxResetCondition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboBoxResetCondition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxResetCondition.FormattingEnabled = true;
			this.comboBoxResetCondition.Location = new System.Drawing.Point(116, 207);
			this.comboBoxResetCondition.Name = "comboBoxResetCondition";
			this.comboBoxResetCondition.Size = new System.Drawing.Size(151, 21);
			this.comboBoxResetCondition.TabIndex = 10;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(8, 210);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(85, 13);
			this.label4.TabIndex = 1;
			this.label4.Text = "Reset Condition:";
			// 
			// ObjectEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(567, 423);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.panel1);
			this.Name = "ObjectEditor";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "ObjectEditor";
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.groupBoxCustomObjectControls.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numberBoxWidth)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numberBoxHeight)).EndInit();
			this.tabPageTileInteractions.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxId;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonDone;
		private System.Windows.Forms.CheckBox checkBoxStartDisabled;
		private System.Windows.Forms.CheckBox checkBoxPoofEffect;
		private System.Windows.Forms.ComboBox comboBoxSolidType;
		private System.Windows.Forms.TabPage tabPageTileInteractions;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Label labelMoveDirection;
		private System.Windows.Forms.CheckBox checkBoxDisableOnDestroy;
		private System.Windows.Forms.Label labelBraceletLevel;
		private System.Windows.Forms.CheckBox checkBoxBurnable;
		private System.Windows.Forms.CheckBox checkBoxBombable;
		private System.Windows.Forms.Label labelSwordLevel;
		private System.Windows.Forms.CheckBox checkBoxDigable;
		private System.Windows.Forms.ComboBox comboBoxMoveDirection;
		private System.Windows.Forms.ComboBox comboBoxBraceletLevel;
		private System.Windows.Forms.ComboBox comboBoxSwordLevel;
		private System.Windows.Forms.CheckBox checkBoxMoveOnce;
		private System.Windows.Forms.CheckBox checkBoxBreakOnSwitch;
		private System.Windows.Forms.CheckBox checkBoxSwitchable;
		private System.Windows.Forms.CheckBox checkBoxBoomerangable;
		private System.Windows.Forms.CheckBox checkBoxPickupable;
		private System.Windows.Forms.CheckBox checkBoxMovable;
		private System.Windows.Forms.CheckBox checkBoxCuttable;
		private System.Windows.Forms.ComboBox comboBoxMovementType;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox comboBoxCollisionModel;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ComboBox comboBoxLedgeDirection;
		private System.Windows.Forms.Label labelLedgeDirection;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.NumericUpDown numberBoxWidth;
		private System.Windows.Forms.NumericUpDown numberBoxHeight;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private ObjectEditorEventsTab eventsTab;
		private System.Windows.Forms.GroupBox groupBoxCustomObjectControls;
		private System.Windows.Forms.Panel panelCustomObjectControl;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private PropertiesEditor.ZeldaPropertyGrid rawPropertyGrid;
		private System.Windows.Forms.Button buttonApply;
		private System.Windows.Forms.ComboBox comboBoxResetCondition;
		private System.Windows.Forms.Label label4;
	}
}